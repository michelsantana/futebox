using Futebox.Models;
using System.Threading.Tasks;

namespace Futebox.Providers
{
    public interface IAudioProvider
    {
        Task<RobotResultApi> GerarAudio(Processo processo, bool buscarDoCacheDownload = false, bool buscarDoCacheArquivos = false);
    }
}
