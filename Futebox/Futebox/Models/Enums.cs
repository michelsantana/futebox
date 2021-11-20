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
        print,
        thumb
    }

    public enum TipoProcesso
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
        Criado = 8,
        Agendado = 16,
        VideoErro = 32,
        VideoCompleto = 64,
        PublicacaoErro = 128,
        Publicado = 256,
        Erro = 512
    }

    public enum RobotResultCommand
    {
        OK,
        ERROR,
        AUTHFAILED,
        BLANK,
        INVALID,
    }
}
