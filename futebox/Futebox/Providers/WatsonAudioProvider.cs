using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.NodeServices;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Providers
{
    public class WatsonAudioProvider : IAudioProvider
    {

        readonly IBrowserService _browserService;

        public WatsonAudioProvider(IBrowserService browserService)
        {
            _browserService = browserService;
        }

        public async Task<RobotResultApi> GerarAudio(Processo processo, bool buscarDoCacheDownload = false, bool buscarDoCacheArquivos = false)
        {
            var result = new RobotResultApi("GerarAudio");
            result.Add("start");

            try
            {
                var arquivoPastaDownload = Path.Combine(Settings.ChromeDefaultDownloadFolder, processo.nomeDoArquivoAudio);
                var arquivoPastaArquivos = Path.Combine(processo.pasta, processo.nomeDoArquivoAudio);

                if (buscarDoCacheDownload && File.Exists(arquivoPastaDownload))
                {
                    FileInfo fi = new FileInfo(arquivoPastaDownload);
                    if (fi.CreationTime > DateTime.Now.AddHours(-8))
                    {
                        result.Add("cache.1");
                        File.Copy(arquivoPastaDownload, Path.Combine(processo.pasta, processo.nomeDoArquivoAudio), true);
                        return result.Ok();
                    }
                    else
                    {
                        File.Move(arquivoPastaDownload, $"{arquivoPastaDownload.Replace(".mp3", "notcache.mp3")}", true);
                    }
                }

                if (buscarDoCacheArquivos && File.Exists(arquivoPastaArquivos))
                {
                    FileInfo fi = new FileInfo(arquivoPastaArquivos);
                    if (fi.CreationTime > DateTime.Now.AddHours(-8))
                    {
                        result.Add("cache.2");
                        return result.Ok();
                    }
                    else
                    {
                        File.Move(arquivoPastaArquivos, $"{arquivoPastaArquivos.Replace(".mp3", "notcache.mp3")}", true);
                    }
                }

                var urlIbm = "https://www.ibm.com/demos/live/tts-demo/self-service/home";

                var page = await _browserService.NewPage();
                result.Add("newpage");

                await page.GoToAsync(urlIbm, WaitUntilNavigation.Networkidle2);
                result.Add("goto");

                await page.WaitForSelectorAsync("audio");
                result.Add("wait");

                await _browserService.RedigitarTextoCampo("#text-area", processo.roteiro, page);

                await page.ClickAsync("#slider");
                _browserService.WaitForMs(700);

                await page.Keyboard.PressAsync("ArrowRight");
                _browserService.WaitForMs(700);

                await page.ClickAsync("#downshift-3-toggle-button");
                _browserService.WaitForMs(700);
                result.Add("config");

                await page.EvaluateExpressionAsync(_browserService.JsFunction("IBMInjetarScriptExtrairAudio", processo.nomeDoArquivoAudio));
                result.Add("inject");

                await page.ClickAsync(".play-btn.bx--btn");
                await page.WaitForSelectorAsync("audio[src]");
                result.Add("play");

                await page.WaitForSelectorAsync("#dwl", new WaitForSelectorOptions() { Timeout = 60000 });
                await page.ClickAsync("#dwl");
                result.Add("download start");

                var tentativas = 1;
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

                File.Copy(arquivoPastaDownload, arquivoPastaArquivos, true);
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
    }
}