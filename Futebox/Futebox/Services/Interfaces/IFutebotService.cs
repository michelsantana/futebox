using Futebox.Models;
using Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IFutebotService
    {
        RobotResultApi VerificarConfiguracaoYoutubeBrowser();
        RobotResultApi VerificarConfiguracaoInstagramBrowser();

        RobotResultApi GerarImagem(SubProcesso subProcesso);
        RobotResultApi GerarAudio(SubProcesso subProcesso);
        RobotResultApi GerarVideo(SubProcesso subProcesso);
        RobotResultApi PublicarVideo(SubProcesso subProcesso);
        RobotResultApi AbrirPasta(Processo subProcesso);

    }
}
