using System;
using System.Collections.Generic;
using System.Linq;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Futebox.Models.Enums;
using Futebox.Pages.Shared;

namespace Futebox.Pages
{
    public class RodadasModel : BaseViewModel
    {
        IRodadaService _service;
        public List<PartidaVM> partidas = new List<PartidaVM>();
        public EnumCampeonato? campeonatoFoco = null;
        public int? rodadaFoco = null;
        public string nomeCampeonato;
        public PageViewModes visualizacao = PageViewModes.padrao;

        public RodadasModel(IRodadaService service)
        {
            _service = service;
        }

        public void OnGet(PageViewModes visualizacao, string campeonato, string rodada)
        {
            this.visualizacao = visualizacao;
            partidas = new List<PartidaVM>();
            if (!string.IsNullOrEmpty(campeonato))
                campeonatoFoco = ((EnumCampeonato)int.Parse(campeonato));
            if (!string.IsNullOrEmpty(rodada))
                rodadaFoco = (int.Parse(rodada));
        }

        public PartialViewResult OnGetRodada(int campeonato, int rodada)
        {
            var enumCampeonato = (EnumCampeonato)campeonato;
            nomeCampeonato = CampeonatoUtils.ObterNomeDoCampeonato(enumCampeonato);
            partidas = _service.ObterPartidasDaRodada(enumCampeonato, rodada, UsarCache(visualizacao))?.ToList();
            rodadaFoco = rodada;
            partidas = partidas.OrderBy(_ => _.dataPartida).ToList();

            return Partial("Templates/_partidaCard", this);
        }
    }
}
