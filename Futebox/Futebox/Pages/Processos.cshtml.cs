using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Pages
{
    public class ProcessosModel : PageModel
    {
        IProcessoService _processoService;
        public List<Processo> processos = new List<Processo>();
        public Dictionary<int, string> cores = new Dictionary<int, string>()
        {
            { (int)Processo.Status.Pendente, "badge bg-warning" },
            { (int)Processo.Status.Sucesso, "badge bg-success" },
            { (int)Processo.Status.Erro, "badge bg-danger" },
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
    }
}
