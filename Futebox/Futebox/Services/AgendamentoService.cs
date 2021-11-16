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

        [DisallowConcurrentExecution]
        public class NotificarJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var jobkey = context.JobDetail.Key;
                Console.WriteLine($"[NOTIFY][START][{jobkey.Name}]");
                Processo processo = null;
                try
                {
                    var service = (INotifyService)context.MergedJobDataMap["service"];
                    processo = (Processo)context.MergedJobDataMap["processo"];

                    service.Notify(processo.notificacao);

                    Console.WriteLine($"[NOTIFY][COMPLETE][{jobkey.Name}]");
                    return Task.FromResult(processo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[NOTIFY][ERROR][{jobkey.Name}]");
                    Console.WriteLine($"{ex.Message}");
                }
                finally
                {
                    Task.WaitAll(context.Scheduler.DeleteJob(jobkey));
                }
                return Task.FromResult(processo);
            }
        }

        [DisallowConcurrentExecution]
        public class ExecutarProcessoJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var jobkey = context.JobDetail.Key;
                Console.WriteLine($"[EXECUTOR][START][{jobkey.Name}]");
                Processo processo = null;
                try
                {
                    processo = (Processo)context.MergedJobDataMap["processo"];
                    var service = (IProcessoService)context.MergedJobDataMap["service"];
                    processo = service.AtualizarRoteiro(processo);
                    service.ExecutarProcesso(processo.id);

                    Console.WriteLine($"[EXECUTOR][COMPLETE][{jobkey.Name}]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EXECUTOR][ERROR][{jobkey.Name}]");
                    Console.WriteLine($"{ex.Message}");
                }
                finally
                {
                    Task.WaitAll(context.Scheduler.DeleteJob(jobkey));
                }
                return Task.FromResult(processo);
            }
        }

        [DisallowConcurrentExecution]
        public class PublicarVideoJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var jobkey = context.JobDetail.Key;
                Console.WriteLine($"[PUBLISH][START][{jobkey.Name}]");
                Processo processo = null;
                try
                {
                    processo = (Processo)context.MergedJobDataMap["processo"];
                    var service = (IProcessoService)context.MergedJobDataMap["service"];
                    processo = service.PublicarVideo(processo.id);

                    Console.WriteLine($"[PUBLISH][COMPLETE][{jobkey.Name}]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PUBLISH][ERROR][{jobkey.Name}]");
                    Console.WriteLine($"{ex.Message}");
                }
                finally
                {
                    Task.WaitAll(context.Scheduler.DeleteJob(jobkey));
                }
                return Task.FromResult(processo);
            }
        }

        public AgendamentoService(IProcessoService processoService, INotifyService notifyService, SchedulerService schedule)
        {
            _processoService = processoService;
            _notifyService = notifyService;
            _schedule = schedule;
        }

        public void AgendarExecucao(string processoId, DateTime date)
        {
            var jobPublishId = JobKey.Create($"publicacao-processo-{processoId}", "geral");
            var jobNotifyId = JobKey.Create($"notificacao-processo-{processoId}", "geral");
            var jobExecuteId = JobKey.Create($"execucao-processo-{processoId}", "geral");

            //AddJob<PublicarVideoJob>(processoId, jobNotifyId, _processoService, date);

            AddJob<ExecutarProcessoJob>(processoId, jobExecuteId, _processoService, date);
            AddJob<PublicarVideoJob>(processoId, jobPublishId, _processoService, date.AddMinutes(5));
            AddJob<NotificarJob>(processoId, jobNotifyId, _notifyService, date.AddMinutes(6));
        }

        private void AddJob<T>(string processoId, JobKey jobId, object service, DateTime date) where T : IJob
        {
            var processo = _processoService.ObterProcesso(processoId);
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("processo", processo);
            dict.Add("service", service);
            dict.Add("jobid", jobId);

            Console.WriteLine($"Agendando processo - {processoId}");

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
