using Futebox.Models;
using Futebox.Providers;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.NodeServices;
using PuppeteerSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public partial class FutebotService : IFutebotService
    {
        readonly IBrowserService _browserService;
        readonly IYoutubeService _youtubeService;
        readonly IInstagramService _instagramService;
        readonly IAudioProvider _audio;

        public FutebotService(
            IBrowserService browserService,
            IYoutubeService youtubeService,
            IInstagramService instagramService,
            IAudioProvider audio)
        {
            _browserService = browserService;
            _youtubeService = youtubeService;
            _instagramService = instagramService;
            _audio = audio;
        }

        public RobotResultApi VerificarConfiguracaoYoutubeBrowser()
        {
            throw new NotImplementedException();
        }
        public RobotResultApi VerificarConfiguracaoInstagramBrowser()
        {
            throw new NotImplementedException();
        }

        private ViewPortOptions Viewport(Processo processo)
        {
            return Viewport(processo.larguraVideo, processo.alturaVideo);
        }
        private ViewPortOptions Viewport(int largura, int altura)
        {
            return new ViewPortOptions()
            {
                Width = largura,
                Height = altura
            };
        }

        public async Task<RobotResultApi> GerarImagem(Processo processo)
        {
            var result = new RobotResultApi("GerarAudio");
            result.Add("start");
            try
            {
                var page = await _browserService.NewPage();
                result.Add("newpage");

                await page.SetViewportAsync(Viewport(processo));
                result.Add("viewport");

                await page.GoToAsync(processo.linkDaImagemDoVideo, WaitUntilNavigation.Networkidle2);
                result.Add("goto");

                _browserService.WaitForMs(7000);
                result.Add("waiting");

                await page.ScreenshotAsync(Path.Combine(processo.pasta, processo.nomeDoArquivoImagem));
                result.Add("screenshot");

                await page.CloseAsync();
                result.Add("close");

                result.Add("end");
                return result.Ok();
            }
            catch (Exception ex)
            {
                result.Add(EyeLog.Log(ex));
                return result.Error();
            }
        }

        public async Task<RobotResultApi> GerarAudio(Processo processo, bool buscarDoCacheDownload = false, bool buscarDoCacheArquivos = false)
        {
            return await _audio.GerarAudio(processo, buscarDoCacheDownload, buscarDoCacheArquivos);
        }

        public RobotResultApi GerarVideo(Processo processo)
        {
            var result = new RobotResultApi("GerarVideo");
            result.Add("start");

            try
            {
                var nomeBaseArquivo = Path.Combine(processo.pasta, $"{processo.social}");
                var titulo = processo.obterTitulo();
                var descricao = processo.obterDescricao();
                var legenda = processo.obterDescricao();

                result.Add("var");

                if (!string.IsNullOrEmpty(titulo)) File.WriteAllText($"{nomeBaseArquivo}.titulo.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{titulo}\")", Encoding.ASCII);
                if (!string.IsNullOrEmpty(descricao)) File.WriteAllText($"{nomeBaseArquivo}.descricao.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{descricao}\")", Encoding.ASCII);
                if (!string.IsNullOrEmpty(legenda)) File.WriteAllText($"{nomeBaseArquivo}.legenda.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{legenda}\")", Encoding.ASCII);

                result.Add("files");

                var arquivoImagem = Path.Combine($"{processo.pasta}", processo.nomeDoArquivoImagem);
                var arquivoAudio = Path.Combine($"{processo.pasta}", processo.nomeDoArquivoAudio);
                var arquivoVideo = Path.Combine($"{processo.pasta}", processo.nomeDoArquivoVideo);

                var arquivoVideoTemp = Path.Combine($"{processo.pasta}", $"{processo.social}.temp.mp4");
                var arquivoShell = Path.Combine($"{processo.pasta}", $"{processo.social}.bat");

                var conteudo = "";
                if (processo.social == Models.Enums.RedeSocialFinalidade.InstagramVideo)
                {
                    conteudo += $"ffmpeg -loop 1 -i {arquivoImagem} -i {arquivoAudio} -c:v libx264 -c:a copy -shortest {arquivoVideoTemp}\n";
                    conteudo += $"ffmpeg -i {arquivoVideoTemp} -c:a aac -b:a 256k -ar 44100 -c:v libx264 -b:v 5M -r 30 -pix_fmt yuv420p -preset faster -tune stillimage {arquivoVideo}";
                }
                else
                {
                    conteudo += $"ffmpeg -loop 1 -i {arquivoImagem} -i {arquivoAudio} -c:v libx264 -c:a copy -shortest {arquivoVideo}";
                }

                File.WriteAllText(arquivoShell, conteudo);
                result.Add("shell");

                Process proc = new Process();
                proc.StartInfo.FileName = arquivoShell;
                proc.StartInfo.WorkingDirectory = processo.pasta;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                proc.WaitForExit();
                result.Add("cmd");

                if (!File.Exists(arquivoVideo)) throw new Exception("File not found");

                result.Add("end");
                return result.Ok();
            }
            catch (Exception ex)
            {
                result.Add(EyeLog.Log(ex));
                return result.Error();
            }
        }
        public async Task<RobotResultApi> PublicarVideo(Processo processo)
        {
            var result = new RobotResultApi("PublicarVideo");
            result.Add("start");
            try
            {

                _browserService.WaitForMs(2);

                if (processo.social == Models.Enums.RedeSocialFinalidade.YoutubeVideo) result = await _youtubeService.Upload(processo);
                if (processo.social == Models.Enums.RedeSocialFinalidade.YoutubeShorts) result = await _youtubeService.Upload(processo);
                if (processo.social == Models.Enums.RedeSocialFinalidade.InstagramVideo) result = await _instagramService.Upload(processo);

                return result;
            }
            catch (Exception ex)
            {
                result.Add(EyeLog.Log(ex));
                return result.Error();
            }
        }

        private async Task<RobotResultApi> PublicarIG(Processo processo)
        {

            var result = new RobotResultApi("PublicarVideo");
            result.Add("start");

            var page = await _browserService.NewPage();
            result.Add("newpage");

            await page.SetViewportAsync(Viewport(1920, 1080));
            result.Add("viewport");

            await page.GoToAsync("https://www.instagram.com/", WaitUntilNavigation.Networkidle2);
            result.Add("goto");

            var loginStatus = await page.EvaluateExpressionAsync(_browserService.JsFunction("IGEstaLogado"));
            if (!loginStatus.ToObject<bool>()) return result.Unauthorized();
            result.Add("login");

            await page.ClickAsync("[aria-label=\"Nova publicação\"]");
            result.Add("upload.1");

            var campoDoUpload = await page.QuerySelectorAsync("[aria-label=\"Criar nova publicação\"] [type=\"file\"]");
            await campoDoUpload.UploadFileAsync(Path.Combine(processo.pasta, processo.nomeDoArquivoVideo));
            _browserService.WaitFor(10);
            result.Add("upload.2");

            await page.EvaluateExpressionAsync(_browserService.JsFunction("IGSelecionarDimensoes"));
            _browserService.WaitFor(1);
            result.Add("aspectratio");

            await page.EvaluateExpressionAsync(_browserService.JsFunction("IGClicarEmProximo"));
            _browserService.WaitFor(1);
            result.Add("next");

            await page.EvaluateExpressionAsync(_browserService.JsFunction("IGClicarEmProximo"));
            _browserService.WaitFor(1);
            result.Add("next");

            await _browserService.RedigitarTextoCampo("textarea[aria-label=\"Escreva uma legenda...\"]", processo.obterDescricao(), page);
            _browserService.WaitFor(1);
            result.Add("caption");

            await page.EvaluateExpressionAsync(_browserService.JsFunction("IGAtivarLegendasAutomaticas"));
            _browserService.WaitFor(1);
            result.Add("autocaption");

            await page.EvaluateExpressionAsync(_browserService.JsFunction("IGPublicar"));
            _browserService.WaitFor(4);
            result.Add("publish");

            await page.CloseAsync();
            result.Add("close");

            result.Add("end");
            return result.Ok();
        }

        public RobotResultApi AbrirPasta(Processo processo)
        {
            return AbrirPasta(processo.pasta);
        }

        public RobotResultApi AbrirPasta(string pasta)
        {
            var result = new RobotResultApi("GerarVideo");
            try
            {
                result.Add("folder start");
                Process proc = new Process();
                proc.StartInfo.FileName = $"explorer.exe {pasta}";
                proc.StartInfo.WorkingDirectory = pasta;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                proc.WaitForExit();
                result.Add("folder opened");
            }
            catch (Exception ex)
            {
                result.Add(EyeLog.Log(ex));
                return result.Error();
            }
            return result.Ok();
        }
    }
}
