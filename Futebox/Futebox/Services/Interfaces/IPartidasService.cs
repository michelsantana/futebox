using Futebox.Models;
using Futebox.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IPartidasService
    {
        IEnumerable<PartidaVM> ObterPartidasHoje(bool skipCache = false);
        IEnumerable<PartidaVM> ObterPartidasDoCampeonato(Campeonatos campeonato, bool clearCache = false);
        string ObterRoteiroDaPartida(PartidaVM partida);
        string ObterRoteiroDaPartida(int idPartida);
        Tuple<string, string> ObterAtributosDoVideo(PartidaVM partida);
        PartidaVM ConverterEmPartidaVM(FootstatsPartida footstatsPartida);
    }
}
