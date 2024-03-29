﻿using Futebox.Models;
using Futebox.Models.Enums;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IFutebotService
    {
        RobotResultApi VerificarConfiguracaoYoutubeBrowser();
        RobotResultApi VerificarConfiguracaoInstagramBrowser();

        Task<RobotResultApi> GerarImagem(Processo processo);
        Task<RobotResultApi> GerarAudio(Processo processo, bool buscarDoCacheDownload = false, bool buscarDoCacheArquivos = false);
        Task<RobotResultApi> GerarVideo(Processo processo);
        Task<RobotResultApi> PublicarVideo(Processo processo);
        Task<RobotResultApi> AbrirPasta(Processo processo);
        Task<RobotResultApi> AbrirPasta(string pasta);
    }
}
