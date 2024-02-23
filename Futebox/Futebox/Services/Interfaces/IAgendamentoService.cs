using Futebox.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IAgendamentoService
    {
        IJobDetail Info(string jobKey);
        Task<List<Agenda>> List();
        Task<List<IJobDetail>> JobList();
        Task<List<IJobExecutionContext>> JobListRunning();
        Task<Agenda> Obter(string processoId);
        Task<Agenda> Criar(string processoId, string descricao = null);
        Task<Agenda> Salvar(Agenda agenda, bool forceCreation = false);
        Task<Agenda> Agendar(string processoId, DateTime data);
        Task<Agenda> Cancelar(string processoId);
        Task InitializeAllJobs();
    }
}
