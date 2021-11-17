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
        Pendente = 8,
        Executando = 16,
        Sucesso = 32,
        Erro = 64
    }

    public enum RobotResultCommand
    {
        RESULT,
        ERROR,
        ISTRUE,
        ISFALSE,
        NONE,
    }
}
