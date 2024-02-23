using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Futebox.Services.Jobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Futebox.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        ISchedulerService _schedule;
        IAgendaRepositorio _agendaRepositorio;

        public AgendamentoService(ISchedulerService schedule, IAgendaRepositorio agendaRepositorio)
        {
            _schedule = schedule;
            _agendaRepositorio = agendaRepositorio;
        }

        public async Task<Agenda> Obter(string processoId)
        {
            try
            {
                var agendaSalva = _agendaRepositorio.GetSingle(_ => _.processoId == processoId);
                if (string.IsNullOrEmpty(agendaSalva?.id)) return null;
                return agendaSalva;
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                return null;
            }
        }

        public async Task<Agenda> Criar(string processoId, string descricao = null)
        {
            try
            {
                var agenda = new Agenda()
                {
                    processoId = processoId,
                    agendamento = null,
                    descricao = descricao,
                    status = Agenda.Status.criado
                };
                return await Salvar(agenda, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Agenda> Salvar(Agenda agenda, bool forceCreation = false)
        {
            var agendaSalva = _agendaRepositorio.GetSingle(_ => _.processoId == agenda.processoId);

            if (string.IsNullOrEmpty(agendaSalva?.id))
            {
                if (forceCreation)
                    _agendaRepositorio.Insert(ref agenda);
                else throw new Exception("agenda não encontrada");
            }
            else agenda = _agendaRepositorio.UpdateReturn(agenda);
            return agenda;
        }

        public async Task<Agenda> Agendar(string processoId, DateTime data)
        {
            var agendaSalva = _agendaRepositorio.GetSingle(_ => _.processoId == processoId);
            if (string.IsNullOrEmpty(agendaSalva?.id)) throw new Exception("Agenda não criada");

            _agendaRepositorio.OpenTransaction();
            agendaSalva.agendamento = data;
            agendaSalva.status = Agenda.Status.agendado;
            agendaSalva = _agendaRepositorio.UpdateReturn(agendaSalva);
            _agendaRepositorio.Commit();
            AddJob<HttpJob>(agendaSalva.processoId, data);

            return agendaSalva;
        }

        public async Task<Agenda> Cancelar(string processoId)
        {
            var agendaSalva = _agendaRepositorio.GetSingle(_ => _.processoId == processoId);
            if (string.IsNullOrEmpty(agendaSalva.id)) throw new Exception("Agenda não criada");

            _agendaRepositorio.OpenTransaction();
            agendaSalva.agendamento = null;
            agendaSalva.status = Agenda.Status.cancelado;
            agendaSalva = _agendaRepositorio.UpdateReturn(agendaSalva);
            _agendaRepositorio.Commit();
            _schedule.RemoveJob(agendaSalva.processoId);

            return agendaSalva;
        }

        public async Task<List<Agenda>> List()
        {
            return _agendaRepositorio.GetAll()?.ToList();
        }

        public IJobDetail Info(string jobKey)
        {
            return _schedule.Info(jobKey);
        }

        public async Task<List<IJobDetail>> JobList()
        {
            return await _schedule.List();
        }

        public async Task<List<IJobExecutionContext>> JobListRunning()
        {
            return await _schedule.ListRunning();
        }

        private void AddJob<T>(string processoId, DateTime date) where T : IJob
        {
            var jobId = new JobKey(processoId);
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("processo", processoId);
            dict.Add("jobid", jobId);

            EyeLog.Log($"Agendando processo - {processoId}");

            IJobDetail job = JobBuilder.Create<T>()
                .WithIdentity(jobId)
                .StoreDurably(false)
                .SetJobData(new JobDataMap(dict))
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(jobId.Name, jobId.Group)
                .StartAt(date)
                .Build();

            _schedule.AddJob(job, trigger);
        }

        public async Task InitializeAllJobs()
        {
            var found = _agendaRepositorio.GetList(_ => _.status == Agenda.Status.agendado);
            found.ToList().ForEach(_ =>
            {
                if (_.agendamento.HasValue)
                    AddJob<HttpJob>(_.processoId, _.agendamento.Value);
            });
        }
    }
}
