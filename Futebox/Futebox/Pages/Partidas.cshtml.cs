using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Futebox.Pages
{
    public class PartidasModel : PageModel
    {
        IPartidasService _calendarioService;
        public List<PartidaVM> partidas = new List<PartidaVM>();
        public PartidaVM partidaFoco = null;

        public PartidasModel(IPartidasService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        public void OnGet(string partida, string printMode)
        {
            var isPrintMode = !string.IsNullOrEmpty(printMode);
            this.partidas = _calendarioService.ObterPartidasHoje(isPrintMode)?.ToList();
            if (!string.IsNullOrEmpty(partida))
                this.partidaFoco = this.partidas.Find(_ => _.idExterno.ToString() == partida);
        }

        public PartialViewResult OnGetFocoPartida(string p)
        {
            this.partidas = _calendarioService.ObterPartidasHoje()?.ToList();
            this.partidaFoco = this.partidas.Find(_ => _.idExterno.ToString() == p);
            return Partial("Templates/_partidaFoco", this.partidaFoco);
        }
    }
}
