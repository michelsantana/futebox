using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Pages.Shared;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Pages
{
    public class PartidasModel : BaseViewModel
    {
        IPartidasService _calendarioService;

        public List<PartidaVM> partidas = new List<PartidaVM>();
        public PartidaVM partida = null;
        
        public PageViewModes qsPageViewMode = PageViewModes.padrao;
        public string qsPartidaId = null;

        public PartidasModel(IPartidasService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        public void OnGet(string partidaId, PageViewModes viewMode)
        {
            qsPageViewMode = viewMode;
            qsPartidaId = partidaId;

            partidas = _calendarioService.ObterPartidasHoje(viewMode == PageViewModes.print)?.ToList();
        }

        public PartialViewResult OnGetFocoPartida(string partidaId, PageViewModes viewMode)
        {
            qsPartidaId = partidaId;
            qsPageViewMode = viewMode;

            partidas = _calendarioService.ObterPartidasHoje()?.ToList();
            partida = partidas.Find(_ => _.idExterno.ToString() == partidaId);

            return Partial("Templates/_partidaFoco", this);
        }

    }
}
