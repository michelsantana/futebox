using Futebox.Models;
using System;
using System.Collections.Generic;

namespace Futebox.Services.Interfaces
{
    public interface IRodadaService
    {
        public List<PartidaVM> ObterPartidasDaRodada(Models.Enums.EnumCampeonato campeonato, int rodada, bool usarCache = true);
        string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, ProcessoRodadaArgs processoRodadaArgs);
        Tuple<string, string> ObterAtributosDoVideo(ProcessoRodadaArgs processoRodadaArgs);
    }
}
