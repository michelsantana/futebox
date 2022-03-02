using Futebox.Models;
using Futebox.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class InstagramService : IInstagramService
    {
        readonly IBrowserService _browserService;
        private Page _page;

        public InstagramService(IBrowserService browserService)
        {
            _browserService = browserService;
        }

        private ViewPortOptions Viewport(int largura, int altura)
        {
            return new ViewPortOptions()
            {
                Width = largura,
                Height = altura
            };
        }

        public async Task Abrir()
        {
            _page = await _browserService.NewPage();
            await _page.SetViewportAsync(Viewport(1080, 720));
            await _page.GoToAsync("https://www.instagram.com/", WaitUntilNavigation.Networkidle2);
            _browserService.WaitFor(1);
        }

        public async Task<bool> EstaLogado()
        {
            var loginStatus = await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGEstaLogado"));
            _browserService.WaitFor(1);
            if (!loginStatus.ToObject<bool>()) return false;
            return true;
        }

        public async Task AbrirUpload()
        {
            await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGNaoPermiteNotificacao"));
            _browserService.WaitFor(1);

            await _page.ClickAsync("[aria-label=\"Nova publicação\"]");
            _browserService.WaitFor(1);
        }

        public async Task EfetuarUploadArquivo(string arquivo)
        {
            if (!File.Exists(arquivo)) throw new Exception("File not found");

            var campoDoUpload = await _page.QuerySelectorAsync("[aria-label=\"Criar nova publicação\"] [type=\"file\"]");
            await campoDoUpload.UploadFileAsync(arquivo);
            _browserService.WaitFor(10);
        }

        public async Task SelecionarDimensaoVideo()
        {
            await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGSelecionarDimensoes"));
            _browserService.WaitFor(1);
        }

        public async Task ClicarEmProximo()
        {
            await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGClicarEmProximo"));
            _browserService.WaitFor(1);
        }

        public async Task AtivarLegendasAutomaticas()
        {
            await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGAtivarLegendasAutomaticas"));
            _browserService.WaitFor(1);
        }

        public async Task ClicarEmPublicar()
        {
            if(!Settings.DEBUGMODE) await _page.EvaluateExpressionAsync(_browserService.JsFunction("IGPublicar"));
            _browserService.WaitFor(10);
        }

        public async Task Fechar()
        {
            await _page?.CloseAsync();
            _page = null;
        }

        public Page GetPage()
        {
            return _page;
        }

        public async Task<RobotResultApi> Upload(Processo processo)
        {

            var result = new RobotResultApi("Upload");
            result.Add("start");

            try
            {
                await Abrir();
                result.Add("accessing");

                var estaLogado = await EstaLogado();
                if (!estaLogado) return result.Unauthorized();
                result.Add("login");

                await AbrirUpload();
                result.Add("upload.1");

                await EfetuarUploadArquivo(Path.Combine(processo.pasta, processo.nomeDoArquivoVideo));
                result.Add("upload.2");

                await SelecionarDimensaoVideo();
                result.Add("aspectratio");

                await ClicarEmProximo();
                result.Add("next");

                await ClicarEmProximo();
                result.Add("next");

                await _browserService.RedigitarTextoCampo("textarea[aria-label=\"Escreva uma legenda...\"]", processo.obterDescricao(), _page);
                _browserService.WaitFor(1);
                result.Add("caption");

                await AtivarLegendasAutomaticas();
                result.Add("autocaption");

                await ClicarEmPublicar();
                result.Add("publish");

                await Fechar();
                result.Add("close");

                return result.Ok();
            }
            catch (Exception ex)
            {
                await Fechar();
                result.Add(EyeLog.Log(ex));
                return result.Error();
            }
        }
    }
}
