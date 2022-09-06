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
        readonly INodeServices _node;

        public FutebotService(
            IBrowserService browserService,
            IYoutubeService youtubeService,
            IInstagramService instagramService,
            IAudioProvider audio,
            INodeServices node)
        {
            _browserService = browserService;
            _youtubeService = youtubeService;
            _instagramService = instagramService;
            _audio = audio;
            _node = node;
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

        public async Task<RobotResultApi> GerarVideo(Processo processo)
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
                    conteudo += $"ffmpeg -y -loop 1 -i {arquivoImagem} -i {arquivoAudio} -c:v libx264 -c:a copy -shortest {arquivoVideoTemp}\n";
                    conteudo += $"ffmpeg -y -i {arquivoVideoTemp} -c:a aac -b:a 256k -ar 44100 -c:v libx264 -b:v 5M -r 30 -pix_fmt yuv420p -preset faster -tune stillimage {arquivoVideo}";
                }
                else
                {
                    conteudo += $"ffmpeg -y -loop 1 -i {arquivoImagem} -i {arquivoAudio} -c:v libx264 -c:a copy -shortest {arquivoVideo}";
                }

                File.WriteAllText(arquivoShell, conteudo);
                result.Add("shell");

                var r = await _node.InvokeAsync<NodeServiceResult>($"{Settings.ApplicationRoot}/NodeServices/src/cmd.js", new object[] { arquivoShell });
                if (!r.status) throw new Exception(string.Join("|", r.steps));

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

        public async Task<RobotResultApi> AbrirPasta(Processo processo)
        {
            return await AbrirPasta(processo.pasta);
        }

        public async Task<RobotResultApi> AbrirPasta(string pasta)
        {
            var result = new RobotResultApi("GerarVideo");
            try
            {
                result.Add("folder start");
                var r = await _node.InvokeAsync<NodeServiceResult>($"{Settings.ApplicationRoot}/NodeServices/src/cmd.js", new object[] { $"explorer.exe {pasta.Replace("/", "\\")}" });
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

    //{
    // outra forma de executar o cmd
    //Process proc = new Process();
    //proc.StartInfo.FileName = arquivoShell;
    //proc.StartInfo.WorkingDirectory = processo.pasta;
    //proc.StartInfo.UseShellExecute = true;
    //proc.Start();
    //proc.WaitForExit();
    //}
}
