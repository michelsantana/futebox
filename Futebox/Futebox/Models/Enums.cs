using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models.Enums
{
    public enum Campeonatos
    {
        BrasileiraoSerieA = 767,
        BrasileiraoSerieB = 769,
        Libertadores2021 = 771
    }

    public enum OrigemDado
    {
        footstats
    }

    public enum PageViewModes
    {
        padrao,
        igv,
        yts,
        ytv,
    }

    public enum CategoriaVideo
    {
        partida,
        classificacao,
        rodada,
    }

    public enum TipoLink
    {
        print,
        image
    }

    public enum StatusProcesso
    {
        Criado = 0,
        Agendado = 10,

        GerandoImagem = 20,
        ImagemErro = 25,
        ImagemOK = 29,

        GerandoAudio = 30,
        AudioErro = 35,
        AudioOK = 39,

        GerandoVideo = 40,
        VideoErro = 45,
        VideoOK = 49,

        Publicando = 50,
        PublicandoErro = 55,
        PublicandoOK = 59,

        Erro = 100
    }

    public enum RobotResultCommand
    {
        OK,
        ERROR,
        AUTHFAILED,
        BLANK,
        INVALID,
    }

    public enum RedeSocialFinalidade
    {
        NENHUMA,
        YoutubeShorts,
        YoutubeVideo,
        InstagramVideo
    }
}
