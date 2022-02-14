using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class InstagramService : BrowserService
    {
        IQueueService _queueService;
        ICacheHandler _cache;
        INodeServices _node;

        public InstagramService(IQueueService queueService, ICacheHandler cache, INodeServices node) : base()
        {
            _queueService = queueService;
            _cache = cache;
            _node = node;
        }

        public async Task PrepareImage(SubProcesso subProcesso)
        {
            var file = Path.Combine(subProcesso.pastaDoArquivo, subProcesso.nomeDoArquivoImagem);
            if (!File.Exists(file))
            {
                var page = await _browser.NewPageAsync();
                await page.SetViewportAsync(new PuppeteerSharp.ViewPortOptions()
                {
                    Height = subProcesso.alturaVideo,
                    Width = subProcesso.larguraVideo,
                });
                await page.GoToAsync(subProcesso.linkDaImagemDoVideo, PuppeteerSharp.WaitUntilNavigation.Networkidle2);
                WaitFor(5);
                await page.ScreenshotAsync(file);
            }
        }

        public async Task PostPhotoOnInstagram(SubProcesso subProcesso)
        {
            await this.Enqueue(() => FnPostPhotoOnInstagram(post));
        }
        private async Task FnPostPhotoOnInstagram(SubProcesso subProcesso)
        {
            Browser browser = null;
            Page page = null;
            try
            {
                var cropedImage = await PrepareImage(post);
                browser = await OpenBrowser();
                page = await browser.NewPageAsync();
                await page.SetViewportAsync(GetDefaultViewPort());

                await page.GoToAsync("https://www.instagram.com");

                Utils.WaitFor(2);
                var instagramIsLoggedAtBrowser = await page.EvaluateExpressionAsync<bool>(await LoadScript("instagram-scripts.js", "Logged();"));

                if (!instagramIsLoggedAtBrowser)
                {
                    var message = "O usuário não está logado no browser/perfil utilizado. Deve fazer o login antes de fazer postagens";
                    EyeLog.Log(message);
                    throw ServiceEx.Unauthorized(message);
                }

                await page.ClickAsync("[aria-label=\"Nova publicação\"]");
                Utils.WaitFor(2);

                var elementUploadHandle = await page.QuerySelectorAsync("[aria-label=\"Criar nova publicação\"] [type=\"file\"]");
                await elementUploadHandle.UploadFileAsync($"{cropedImage.output[0]}");
                Utils.WaitFor(10);

                await page.EvaluateExpressionAsync(await LoadScript("instagram-scripts.js", "AspectRatio();"));
                Utils.WaitFor(2);

                var repeat = 0;
                do
                {
                    repeat++;
                    await page.EvaluateExpressionAsync(await LoadScript("instagram-scripts.js", "Next();"));
                    Utils.WaitFor(1);
                } while (repeat < 2);

                await ClearInputThenWriteText(page, "textarea[aria-label=\"Escreva uma legenda...\"]", "Siga-me vamos degustar memes");

                await page.EvaluateExpressionAsync(await LoadScript("instagram-scripts.js", "Publish();"));
                Utils.WaitFor(10);

                await page.CloseAsync();
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex.Message);
                await page?.CloseAsync();
                throw ServiceEx.Error(ex.Message);
            }
        }

        public async Task GetInstagramLoggedStatus(bool force = false)
        {
            await this.Enqueue(() => FnGetInstagramLoggedStatus(force));
        }
        private async Task FnGetInstagramLoggedStatus(bool force)
        {
            Browser browser = null;
            Page page = null;
            var instagramCachedStatus = await _cache.GetData<ServiceStatus?>(Settings.CacheNames.InstagramLastStatusTime);

            if (instagramCachedStatus.HasValue) app.instagramStatus = instagramCachedStatus.Value;
            if (app.instagramStatus == ServiceStatus.Online && !force) return;

            try
            {
                browser = await OpenBrowser();
                page = await browser.NewPageAsync();
                await page.SetViewportAsync(GetDefaultViewPort());

                await page.GoToAsync("https://www.instagram.com", WaitUntilNavigation.Networkidle0);

                var instagramIsLoggedAtBrowser = await page.EvaluateExpressionAsync<bool>(await LoadScript("instagram-scripts.js", "Logged();"));
                await page.ScreenshotAsync($"{Settings.ArchiveFolder}/last-instagram-test.png");

                if (!instagramIsLoggedAtBrowser) app.instagramStatus = ServiceStatus.Offline;
                else app.instagramStatus = ServiceStatus.Online;

                _cache.SetData(Settings.CacheNames.InstagramLastStatusTime, app.instagramStatus, 48);

                await page?.CloseAsync();
            }
            catch (Exception ex)
            {
                await page?.CloseAsync();
                app.instagramStatus = ServiceStatus.Error;
                EyeLog.Log(ex.Message);
            }
        }
    }
}
