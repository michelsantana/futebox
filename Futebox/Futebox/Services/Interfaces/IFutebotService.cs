using Futebox.Models;
using Futebox.Models.Enums;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IFutebotService
    {
        RobotResultApi VerificarConfiguracaoYoutubeBrowser();
        RobotResultApi VerificarConfiguracaoInstagramBrowser();

        Task<RobotResultApi> GerarImagem(SubProcesso subProcesso);
        Task<RobotResultApi> GerarAudio(SubProcesso subProcesso);
        RobotResultApi GerarVideo(SubProcesso subProcesso);
        Task<RobotResultApi> PublicarVideo(SubProcesso subProcesso);
        RobotResultApi AbrirPasta(Processo subProcesso);

    }
}
