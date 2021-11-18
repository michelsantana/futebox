using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IFutebotService
    {
        RobotResult VerificarConfiguracaoYoutubeBrowser();
        RobotResult GerarVideo(string processoId);
        RobotResult PublicarVideo(string processoId);
        RobotResult AbrirPasta(string processoId);

    }
}
