using Futebox.Models;
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
        public Dictionary<int, string> cores = new Dictionary<int, string>()
        {
            { (int)Processo.Status.Pendente, "badge bg-warning" },
            { (int)Processo.Status.Executando, "badge bg-info" },
            { (int)Processo.Status.Sucesso, "badge bg-success" },
            { (int)Processo.Status.Erro, "badge bg-danger" },
            { 1, "badge bg-warning" },
        };

        public ProcessosModel(IProcessoService processoService)
        {
            _processoService = processoService;
        }

        public void OnGet()
        {
            this.processos = _processoService.ObterProcessos()?.ToList();
            this.processos = this.processos.OrderBy(_ => (_.criacao.Ticks * _.status) * -1).ToList();
        }

        public PartialViewResult OnGetProcessoTableRow(string processoId)
        {
            var processo = _processoService.ObterProcesso(processoId);
            return Partial("Templates/_processoTableRow", Tuple.Create(this, processo));
        }
    }
}
