using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IAgendamentoService
    {
        void AgendarExecucao(string processoId, DateTime date);
    }
}
