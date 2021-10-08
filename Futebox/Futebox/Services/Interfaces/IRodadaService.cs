using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IRodadaService
    {
        public List<PartidaVM> ObterPartidasDaRodada(Campeonatos campeonato, int rodada, bool skipCache = false);
        string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, Campeonatos campeonato, int rodada);
        Tuple<string, string> ObterAtributosDoVideo(IEnumerable<PartidaVM> partidas, Campeonatos campeonato, int rodada);
    }
}
