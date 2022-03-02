using Futebox.Models;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IYoutubeService
    {
        Task Abrir();
        Task<bool> EstaLogado();
        Task AbrirUpload();
        Task EfetuarUploadArquivo(string arquivo);
        Task SelecionarPlayList(string categoria);
        Task ClicarEmProximo();
        Task SelecionarPrivacidade();
        Task ClicarEmPublicar();
        Task Fechar();
        Page GetPage();
        Task<RobotResultApi> Upload(Processo processo);
    }
}
