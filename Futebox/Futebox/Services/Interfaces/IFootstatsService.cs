using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IFootstatsService
    {
        List<FootstatsTime> ObterTimesServico();
        List<FootstatsClassificacao> ObterClassificacaoServico(Models.Enums.EnumCampeonato campeonato);
        FootstatsPartida ObterPartida(int partida);
        List<FootstatsPartida> ObterPartidasPeriodo();
        List<FootstatsPartida> ObterPartidasHoje();
        List<FootstatsPartida> ObterPartidasDaRodada(Models.Enums.EnumCampeonato campeonato, int rodada);
        List<FootstatsPartida> ObterPartidasDoCampeonato(Models.Enums.EnumCampeonato campeonato);
    }
}
