using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IFootstatsService
    {
        List<FootstatsTime> ObterTimesServico();
        List<FootstatsClassificacao> ObterClassificacaoServico(Campeonatos campeonato);
        List<FootstatsPartida> ObterPartidasHoje();
        List<FootstatsPartida> ObterPartidasDaRodada(Campeonatos campeonato, int rodada);
    }
}
