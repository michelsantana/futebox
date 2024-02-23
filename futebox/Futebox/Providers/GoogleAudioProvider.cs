using Futebox.Models;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Providers
{
    public class GoogleAudioProvider : IAudioProvider
    {

        readonly INodeServices _nodeServices;

        public GoogleAudioProvider(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
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

                var r = await _nodeServices.InvokeAsync<NodeServiceResult>($"{Settings.ApplicationRoot}/NodeServices/src/tts.js", new object[] { processo.id, processo.roteiro, arquivoPastaDownload });
                if (!r.status) throw new Exception(string.Join("|", r.steps));

                File.Copy(arquivoPastaDownload, arquivoPastaArquivos, true);

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
