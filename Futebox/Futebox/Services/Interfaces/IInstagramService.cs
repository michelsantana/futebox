using Futebox.Models;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IInstagramService
    {
        Task Abrir();
        Task<bool> EstaLogado();
        Task AbrirUpload();
        Task EfetuarUploadArquivo(string arquivo);
        Task SelecionarDimensaoVideo();
        Task ClicarEmProximo();
        Task AtivarLegendasAutomaticas();
        Task ClicarEmPublicar();
        Task Fechar();
        Page GetPage();
        Task<RobotResultApi> Upload(Processo processo);
    }
}
