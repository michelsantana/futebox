using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Pages.Shared;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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

            partidas = _calendarioService.ObterPartidasPeriodo(UsarCache(viewMode))?.ToList();
            //partidas = _calendarioService.ObterPartidasAntigas("FootstatsPartida20211110").ToList();
        }

        public PartialViewResult OnGetFocoPartida(string partidaId, PageViewModes viewMode)
        {
            qsPartidaId = partidaId;
            qsPageViewMode = viewMode;

            //partidas = _calendarioService.ObterPartida(Convert.ToInt32(partidaId))?.ToList();
            partida = _calendarioService.ObterPartida(Convert.ToInt32(partidaId), UsarCache(viewMode));

            if(viewMode == PageViewModes.igv)
                return Partial("Templates/_partidaFocoIG", this);
            if(viewMode == PageViewModes.yts)
                return Partial("Templates/_partidaFocoYT", this);
            if (viewMode == PageViewModes.ytv)
                return Partial("Templates/_partidaFocoYT", this);

            return Partial("Templates/_partidaFocoYT", this);
        }

    }
}
