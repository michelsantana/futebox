﻿using Futebox.Models;
using System;
using System.Collections.Generic;

namespace Futebox.Services.Interfaces
{
    public interface IRodadaService
    {
        public List<PartidaVM> ObterPartidasDaRodada(Models.Enums.EnumCampeonato campeonato, int rodada, bool usarCache = true);
        Tuple<string, string> ObterAtributosDoVideo(ProcessoRodadaArgs processoRodadaArgs);
    }
}
