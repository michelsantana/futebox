using Futebox.Models;
using Futebox.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class YoutubeService : IYoutubeService
    {

        readonly IBrowserService _browserService;
        private Page _page;

        public YoutubeService(IBrowserService browserService)
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
            await _page.GoToAsync("https://studio.youtube.com/channel/UCWs2h6plWKR8xCZM3ljNGRw", WaitUntilNavigation.Networkidle2);
            _browserService.WaitFor(1);
        }

        public async Task<bool> EstaLogado()
        {
            var loginStatus = await _page.EvaluateExpressionAsync(_browserService.JsFunction("YTEstaLogado"));
            _browserService.WaitFor(1);
            if (!loginStatus.ToObject<bool>()) return false;
            return true;
        }

        public async Task AbrirUpload()
        {
            await _page.ClickAsync("#create-icon");
            _browserService.WaitFor(1);

            await _page.ClickAsync("#text-item-0");
            _browserService.WaitFor(1);
        }

        public async Task EfetuarUploadArquivo(string arquivo)
        {
            if (!File.Exists(arquivo)) throw new Exception("File not found");

            var campoDoUpload = await _page.QuerySelectorAsync("[type=\"file\"]");
            await campoDoUpload.UploadFileAsync(arquivo);
            _browserService.WaitFor(10);
        }

        public async Task SelecionarPlayList(string categoria)
        {
            return;
            await _page.ClickAsync(".dropdown.style-scope.ytcp-video-metadata-playlists");
            _browserService.WaitFor(1);

            var clickId = await _page.EvaluateExpressionAsync(_browserService.JsFunction("YTSelecionarPlaylist", $"{categoria}"));
            await _page.ClickAsync($"{clickId.ToObject<string>()}");
            _browserService.WaitFor(1);

            await _page.ClickAsync(".done-button.action-button.style-scope.ytcp-playlist-dialog");
            _browserService.WaitFor(1);
        }

        public async Task ClicarEmProximo()
        {
            var seletorRodapeJanela = ".button-area.ytcp-uploads-dialog";
            await _page.ClickAsync($"{seletorRodapeJanela} #next-button");
            _browserService.WaitFor(3);
        }

        public async Task SelecionarPrivacidade()
        {
            if (Settings.DEBUGMODE) await _page.ClickAsync("[name=\"PRIVATE\"]");
            _browserService.WaitFor(3);
        }

        public async Task ClicarEmPublicar()
        {
            var seletorRodapeJanela = ".button-area.ytcp-uploads-dialog";
            await _page.ClickAsync($"{seletorRodapeJanela} #done-button");
            _browserService.WaitFor(10);
        }

        public async Task Fechar()
        {
            await _page.CloseAsync();
            _page = null;
        }

        public Page GetPage()
        {
            return _page;
        }

        public async Task<RobotResultApi> Upload(Processo processo)
        {
            var result = new RobotResultApi("PublicarVideo");
            result.Add("start");

            try
            {
                await Abrir();
                result.Add("newpage");

                var estaLogado = await EstaLogado();
                if (!estaLogado) return result.Unauthorized();
                result.Add("login");

                await AbrirUpload();
                result.Add("upload.1");

                await EfetuarUploadArquivo(Path.Combine(processo.pasta, processo.nomeDoArquivoVideo));
                result.Add("upload.2");

                await _browserService.RedigitarTextoCampo(".input-container.title #textbox", processo.obterTitulo(), _page);
                _browserService.WaitFor(1);
                await _page.Keyboard.PressAsync("Escape");
                result.Add("title");

                await _browserService.RedigitarTextoCampo(".input-container.description #textbox", processo.obterDescricao(), _page);
                _browserService.WaitFor(1);
                await _page.Keyboard.PressAsync("Escape");
                result.Add("description");

                await SelecionarPlayList($"{processo.categoria}");
                result.Add("playlist.1");

                await ClicarEmProximo();
                await ClicarEmProximo();
                await ClicarEmProximo();
                result.Add("next");

                await SelecionarPrivacidade();
                result.Add("visibility");

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
