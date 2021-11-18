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
        public class ExecutarProcessoJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                var jobkey = context.JobDetail.Key;

                try
                {
                    var _processoService = (IProcessoService)context.MergedJobDataMap[nameof(IProcessoService)];
                    var _notifyService = (INotifyService)context.MergedJobDataMap[nameof(INotifyService)];
                    var processo = (Processo)context.MergedJobDataMap["processo"];

                    EyeLog.Log($"[VIDEO][START][{jobkey.Name}]");
                    _processoService.AtualizarRoteiro(processo);
                    _processoService.GerarVideoProcesso(processo.id);
                    EyeLog.Log($"[VIDEO][COMPLETE][{jobkey.Name}]");

                    EyeLog.Log($"[PUBLISH][START][{jobkey.Name}]");
                    _processoService.PublicarVideo(processo.id);
                    EyeLog.Log($"[PUBLISH][COMPLETE][{jobkey.Name}]");

                    EyeLog.Log($"[NOTIFY][START][{jobkey.Name}]");
                    _notifyService.Notify(processo.notificacao);
                    EyeLog.Log($"[NOTIFY][COMPLETE][{jobkey.Name}]");
                }
                catch (Exception ex)
                {
                    EyeLog.Log($"[ERROR][{jobkey.Name}]");
                    EyeLog.Log($"{ex.Message}");
                }
                finally
                {
                    Task.WaitAll(context.Scheduler.DeleteJob(jobkey));
                }
                return Task.FromResult(true);
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

            var jobExecuteId = JobKey.Create($"execucao-processo-{processoId}", "geral");

            //AddJob<PublicarVideoJob>(processoId, jobNotifyId, _processoService, date);

            AddJob<ExecutarProcessoJob>(processoId, jobExecuteId, date);
        }

        private void AddJob<T>(string processoId, JobKey jobId, DateTime date) where T : IJob
        {
            var processo = _processoService.ObterProcesso(processoId);
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("processo", processo);
            dict.Add(nameof(IProcessoService), _processoService);
            dict.Add(nameof(INotifyService), _notifyService);
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
    }
}
