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
    public class JogosDiaModel : BaseViewModel
    {
        IPartidasService _service;

        public EnumCampeonato? campeonatoFoco = null;
        public string nomeCampeonato;
        public PageViewModes visualizacao = PageViewModes.padrao;
        public List<PartidaVM> partidas;

        public JogosDiaModel(IPartidasService service)
        {
            _service = service;
        }

        public void OnGet(PageViewModes visualizacao, string campeonato, string rodada)
        {
            this.visualizacao = visualizacao;
            if (!string.IsNullOrEmpty(campeonato)) campeonatoFoco = ((EnumCampeonato)int.Parse(campeonato));
            partidas = _service.ObterPartidasPeriodo(false)?.ToList();
            partidas = partidas.OrderBy(_ => _.dataPartida).ToList();
        }

        //public PartialViewResult OnGetJogos()
        //{
        //    var partidasVm = _service.ObterPartidasPeriodo(false)?.ToList();
        //    partidasVm = partidasVm.OrderBy(_ => _.dataPartida).ToList();

        //    return Partial("JogosDia/_jogosDiaListagem", partidasVm);
        //}
    }
}
