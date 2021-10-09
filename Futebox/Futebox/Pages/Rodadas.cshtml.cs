using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Futebox.Models.Enums;

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

        public void OnGet(string foco, string rodada, string printMode)
        {
            this.clearCache = !string.IsNullOrEmpty(printMode);
            partidas = new List<PartidaVM>();
            if (!string.IsNullOrEmpty(foco))
                this.campeonatoFoco = ((Campeonatos)int.Parse(foco));
            if (!string.IsNullOrEmpty(rodada))
                this.rodadaFoco = (int.Parse(rodada));
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
