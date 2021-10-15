using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Futebox.Models.Enums;

namespace Futebox.Pages
{
    public class RodadasModel : PageModel
    {
        IRodadaService _service;
        public List<PartidaVM> partidas = new List<PartidaVM>();
        public Campeonatos? campeonatoFoco = null;
        public int? rodadaFoco = null;
        public string nomeCampeonato;
        private bool clearCache = false;

        public RodadasModel(IRodadaService service)
        {
            _service = service;
        }

        public void OnGet(string campeonato, string rodada, PageViewModes viewMode)
        {
            clearCache = viewMode == PageViewModes.print;
            partidas = new List<PartidaVM>();
            if (!string.IsNullOrEmpty(campeonato))
                campeonatoFoco = ((Campeonatos)int.Parse(campeonato));
            if (!string.IsNullOrEmpty(rodada))
                rodadaFoco = (int.Parse(rodada));
        }

        public PartialViewResult OnGetRodada(int campeonato, int rodada)
        {
            var enumCampeonato = (Campeonatos)campeonato;
            nomeCampeonato = CampeonatoUtils.ObterNomeDoCampeonato(enumCampeonato);
            partidas = _service.ObterPartidasDaRodada(enumCampeonato, rodada, clearCache)?.ToList();
            rodadaFoco = rodada;
            partidas = partidas.OrderBy(_ => _.dataPartida).ToList();

            return Partial("Templates/_partidaCard", this);
        }
    }
}
