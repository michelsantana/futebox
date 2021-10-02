using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IGerenciamentoTimesService
    {
        List<FootstatsTime> ObterTimesServico();
        List<Time> ObterTimesBancoDados();
        Time SalvarTime(FootstatsTime timeParaSalvar);
        bool RemoverTimeDB(string id);
    }
}
