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

        public ProcessosModel(IProcessoService processoService)
        {
            _processoService = processoService;
        }

        public void OnGet()
        {
            this.processos = _processoService.ObterProcessos()?.ToList();
            this.processos = this.processos.OrderBy(_ => (_.criacao.Ticks) * -1).ToList();
        }

        public PartialViewResult OnGetProcessoTableRow(string processoId)
        {
            var processo = _processoService.ObterProcesso(processoId);
            return Partial("Templates/_processoTableRow", Tuple.Create(this, processo));
        }
    }
}
