using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Providers
{
    public interface IAudioProvider
    {
        Task<RobotResultApi> GerarAudio(Processo processo, bool buscarDoCacheDownload = false, bool buscarDoCacheArquivos = false);
    }
}
