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
        PartidaVM ObterPartida(int idPartida, bool usarCache = true);
        IEnumerable<PartidaVM> ObterPartidasPeriodo(bool usarCache = true);
        IEnumerable<PartidaVM> ObterPartidasHoje(bool usarCache = true);
        IEnumerable<PartidaVM> ObterPartidasDoCampeonato(EnumCampeonato campeonato, bool usarCache = true);
        IEnumerable<PartidaVM> ObterPartidasAntigas(string cacheFile);
        Tuple<string, string> ObterAtributosDoVideo(PartidaVM partida, ProcessoPartidaArgs processoPartidaArgs);
        PartidaVM ConverterEmPartidaVM(FootstatsPartida footstatsPartida);
    }
}
