using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Pages.Shared;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Pages
{
    public class ProcessosModel : BaseViewModel
    {
        IProcessoService _processoService;
        public List<Processo> processos = new List<Processo>();
        public List<SubProcesso> subprocessos = new List<SubProcesso>();

        public ProcessosModel(IProcessoService processoService)
        {
            _processoService = processoService;
        }

        public void OnGet()
        {
            this.processos = _processoService.ObterProcessos()?.ToList();
            this.processos = this.processos.OrderBy(_ => (_.criacao.Ticks) * -1).ToList();
            this.subprocessos = _processoService.ObterSubProcessos()?.ToList();
        }

        //public PartialViewResult OnGetProcessoTableRow(string processoId)
        //{
        //    var processo = _processoService.ObterProcesso(processoId);
        //    var subprocessos = _processoService.ObterSubProcessos(processoId);
        //    //return Partial("Templates/_processoTableRow", Tuple.Create(this, processo, subprocessos));
        //}
    }
}
