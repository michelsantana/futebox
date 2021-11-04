using Futebox.Models;
using Futebox.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class AgendamentoService : IAgendamentoService
    {
       
        IProcessoService _processoService;
        INotifyService _notifyService;
        SchedulerService _schedule;

        public class NotificarJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var service = (INotifyService)context.MergedJobDataMap["service"];
                var processo = (Processo)context.MergedJobDataMap["processo"];
                var jobName = (JobKey)context.MergedJobDataMap["jobid"];
                Console.WriteLine(processo);

                service.Notify(processo.notificacao);
                
                Task.WaitAll(context.Scheduler.DeleteJob(jobName));
                return Task.FromResult(processo);
            }
        }

        public class ExecutarProcessoJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var service = (IProcessoService)context.MergedJobDataMap["service"];
                var processo = (Processo)context.MergedJobDataMap["processo"];
                var jobName = (JobKey)context.MergedJobDataMap["jobid"];
                Console.WriteLine(processo);
                processo = service.AtualizarRoteiro(processo);
                service.ExecutarProcesso(processo.id);

                Task.WaitAll(context.Scheduler.DeleteJob(jobName));
                return Task.FromResult(processo);
            }
        }

        public AgendamentoService(IProcessoService processoService, INotifyService notifyService, SchedulerService schedule)
        {
            _processoService = processoService;
            _notifyService = notifyService;
            _schedule = schedule;
        }

        public void AgendarNotificacao(string processoId, DateTime date)
        {
            var jobNotifyId = JobKey.Create($"notificacao-processo-{processoId}", "geral");
            var jobExecuteId = JobKey.Create($"execucao-processo-{processoId}", "geral");

            AddJob<ExecutarProcessoJob>(processoId, jobExecuteId, _processoService, date);
            AddJob<NotificarJob>(processoId, jobNotifyId, _notifyService, date.AddMinutes(2));
        }

        private void AddJob<T>(string processoId, JobKey jobId, object service, DateTime date) where T : IJob
        {
            var processo = _processoService.ObterProcesso(processoId);
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("processo", processo);
            dict.Add("service", service);
            dict.Add("jobid", jobId);

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
    }
}
