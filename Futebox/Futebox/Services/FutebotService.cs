using Futebox.Models;
using Futebox.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public partial class FutebotService : IFutebotService
    {
        readonly IHttpHandler _http;
        readonly IBrowserService _browserService;

        public FutebotService(IHttpHandler http, IBrowserService browserService)
        {
            _http = http;
            _browserService = browserService;
        }

        public RobotResultApi VerificarConfiguracaoYoutubeBrowser()
        {
            throw new NotImplementedException();
        }
        public RobotResultApi VerificarConfiguracaoInstagramBrowser()
        {
            throw new NotImplementedException();
        }

        private ViewPortOptions Viewport(SubProcesso subProcesso)
        {
            return Viewport(subProcesso.larguraVideo, subProcesso.alturaVideo);
        }
        private ViewPortOptions Viewport(int largura, int altura)
        {
            return new ViewPortOptions()
            {
                Width = largura,
                Height = altura
            };
        }

        public async Task<RobotResultApi> GerarImagem(SubProcesso subProcesso)
        {
            var result = new RobotResultApi("GerarImagem");
            result.Add("start");

            var page = await _browserService.NewPage();
            result.Add("newpage");

            await page.SetViewportAsync(Viewport(subProcesso));
            result.Add("viewport");

            await page.GoToAsync(subProcesso.linkDaImagemDoVideo, WaitUntilNavigation.Networkidle2);
            result.Add("goto");

            _browserService.WaitForMs(7000);
            result.Add("waiting");

            await page.ScreenshotAsync(Path.Combine(subProcesso.pastaDoArquivo, subProcesso.nomeDoArquivoImagem));
            result.Add("screenshot");

            await page.CloseAsync();
            result.Add("close");

            result.Add("end");
            result.Ok();
            return result;
        }

        public async Task<RobotResultApi> GerarAudio(SubProcesso subProcesso)
        {
            var result = new RobotResultApi("GerarAudio");
            result.Add("start");
            try
            {
                var urlIbm = "https://www.ibm.com/demos/live/tts-demo/self-service/home";

                var page = await _browserService.NewPage();
                result.Add("newpage");

                await page.GoToAsync(urlIbm, WaitUntilNavigation.Networkidle2);
                result.Add("goto");

                await page.WaitForSelectorAsync("audio");
                result.Add("wait");

                await RedigitarTextoCampo("#text-area", subProcesso.roteiro, page);

                await page.ClickAsync("#slider");
                _browserService.WaitForMs(700);

                await page.Keyboard.PressAsync("ArrowRight");
                _browserService.WaitForMs(700);

                await page.ClickAsync("#downshift-3-toggle-button");
                _browserService.WaitForMs(700);
                result.Add("config");

                await page.EvaluateExpressionAsync(_browserService.JsFunction("IBMInjetarScriptExtrairAudio", subProcesso.nomeDoArquivoAudio));
                result.Add("inject");

                await page.ClickAsync(".play-btn.bx--btn");
                await page.WaitForSelectorAsync("audio[src]");
                result.Add("play");

                await page.WaitForSelectorAsync("#dwl", new WaitForSelectorOptions() { Timeout = 60000 });
                await page.ClickAsync("#dwl");
                result.Add("download start");

                var tentativas = 1;
                var arquivoPastaDownload = Path.Combine(Settings.ChromeDefaultDownloadFolder, subProcesso.nomeDoArquivoAudio);
                while (tentativas < 12 * 3)
                {
                    if (File.Exists(arquivoPastaDownload))
                    {
                        await page.EvaluateExpressionAsync(_browserService.JsFunction("IBMForcarFinalizarDownload"));
                        result.Add("found");
                        break;
                    }
                    else _browserService.WaitFor(5);
                    tentativas++;
                }
                await page.WaitForSelectorAsync("#dwlend", new WaitForSelectorOptions { Timeout = 60000 * 5 }); // espera até 5 minutos
                result.Add("download end");

                File.Move(arquivoPastaDownload, Path.Combine(subProcesso.pastaDoArquivo, subProcesso.nomeDoArquivoAudio));
                result.Add("file moved");

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

        private async Task RedigitarTextoCampo(string seletor, string texto, Page page)
        {
            await page.ClickAsync(seletor);
            _browserService.WaitForMs(700);

            await page.Keyboard.DownAsync("Control");
            _browserService.WaitForMs(700);

            await page.Keyboard.PressAsync("A");
            _browserService.WaitForMs(700);

            await page.Keyboard.UpAsync("Control");
            _browserService.WaitForMs(700);

            await page.Keyboard.DownAsync("Backspace");
            _browserService.WaitForMs(700);

            await page.Keyboard.TypeAsync(texto);
            _browserService.WaitForMs(700);
        }

        public RobotResultApi GerarVideo(SubProcesso subProcesso)
        {
            var result = new RobotResultApi("GerarVideo");
            result.Add("start");

            var nomeBaseArquivo = Path.Combine(subProcesso.pastaDoArquivo, $"{subProcesso.redeSocial}");
            var titulo = subProcesso.obterTitulo();
            var descricao = subProcesso.obterDescricao();
            var legenda = subProcesso.obterLegenda();

            result.Add("var");

            if (string.IsNullOrEmpty(titulo)) File.WriteAllText($"{nomeBaseArquivo}.titulo.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{titulo}\")", Encoding.ASCII);
            if (string.IsNullOrEmpty(descricao)) File.WriteAllText($"{nomeBaseArquivo}.descricao.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{descricao}\")", Encoding.ASCII);
            if (string.IsNullOrEmpty(legenda)) File.WriteAllText($"{nomeBaseArquivo}.legenda.ps1", $"$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard(\"{legenda}\")", Encoding.ASCII);

            result.Add("files");

            result.Add("end");
            return result.Ok();
        }
        public async Task<RobotResultApi> PublicarVideo(SubProcesso subProcesso)
        {
            var result = new RobotResultApi("PublicarVideo");
            result.Add("start");

            _browserService.WaitForMs(2);



            return result.Ok();
        }

        private async Task<RobotResultApi> PublicarYT(SubProcesso subProcesso)
        {

            var result = new RobotResultApi("PublicarVideo");
            result.Add("start");

            var page = await _browserService.NewPage();
            result.Add("newpage");

            await page.SetViewportAsync(Viewport(1920, 1080));
            result.Add("viewport");

            await page.GoToAsync("https://studio.youtube.com/channel/UCWs2h6plWKR8xCZM3ljNGRw", WaitUntilNavigation.Networkidle2);
            result.Add("goto");

            var loginStatus = await page.EvaluateExpressionAsync(_browserService.JsFunction("YTEstaLogado"));
            if (!loginStatus.ToObject<bool>()) return result.Unauthorized();
            result.Add("login");

            await this.RedigitarTextoCampo(".input-container.title #textbox", subProcesso.obterTitulo(), page);
            _browserService.WaitFor(1);
            result.Add("title");

            await this.RedigitarTextoCampo(".input-container.description #textbox", subProcesso.obterDescricao(), page);
            _browserService.WaitFor(1);
            result.Add("description");

            await page.ClickAsync(".dropdown.style-scope.ytcp-video-metadata-playlists");
            _browserService.WaitFor(1);
            result.Add("playlist.1");

            var clickId = await page.EvaluateExpressionAsync(_browserService.JsFunction("YTSelecionarPlaylist", $"{subProcesso.categoriaVideo}"));
            await page.ClickAsync($"{clickId.ToObject<string>()}");
            _browserService.WaitFor(1);
            result.Add("playlist.2");

            await page.ClickAsync(".done-button.action-button.style-scope.ytcp-playlist-dialog");
            _browserService.WaitFor(1);
            result.Add("playlist.3");

            // obter link do video?

            var seletorRodapeJanela = ".button-area.ytcp-uploads-dialog";

            await page.ClickAsync($"{seletorRodapeJanela} #next-button");
            _browserService.WaitFor(1);
            await page.ClickAsync($"{seletorRodapeJanela} #next-button");
            _browserService.WaitFor(1);
            await page.ClickAsync($"{seletorRodapeJanela} #next-button");
            _browserService.WaitFor(1);
            result.Add("next");

            if (Settings.DEBUGMODE) await page.ClickAsync("[name=\"PRIVATE\"]");
            _browserService.WaitFor(1);

            await page.ClickAsync($"{seletorRodapeJanela} #done-button");
            _browserService.WaitFor(4);
            result.Add("publish");

            await page.CloseAsync();
            result.Add("close");

            result.Add("end");
            return result.Ok();
        }

        private async Task<RobotResultApi> PublicarIG(SubProcesso subProcesso)
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
            await campoDoUpload.UploadFileAsync($"{subProcesso.pastaDoArquivo}/{subProcesso.nomeDoArquivoVideo}");
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

            await this.RedigitarTextoCampo("textarea[aria-label=\"Escreva uma legenda...\"]", subProcesso.obterLegenda(), page);
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
            throw new NotImplementedException();
        }

        Task<RobotResultApi> IFutebotService.PublicarVideo(SubProcesso subProcesso)
        {
            throw new NotImplementedException();
        }
    }
}
