using Futebox.Services.Interfaces;
using Futebox.Services.Jobs;
using Quartz;
using System;
using System.Collections.Generic;

namespace Futebox.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        ISchedulerService _schedule;

        public AgendamentoService(ISchedulerService schedule)
        {
            _schedule = schedule;
        }

        public void AgendarExecucao(string processoId, DateTime date)
        {
            var jobExecuteId = JobKey.Create($"execucao-processo-{processoId}", "geral");
            AddJob<QueueJob>(processoId, jobExecuteId, date);
        }

        private void AddJob<T>(string processoId, JobKey jobId, DateTime date) where T : IJob
        {
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
    }
}
