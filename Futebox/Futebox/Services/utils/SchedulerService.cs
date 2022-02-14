using Futebox.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class SchedulerService : ISchedulerService
    {
        static StdSchedulerFactory _factory;
        static IScheduler _scheduler;
        readonly IServiceProvider _serviceProvider;

        public SchedulerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _factory = _factory ?? new StdSchedulerFactory();
            _scheduler = _scheduler ?? _factory.GetScheduler().Result;
            if (!_scheduler.IsStarted)
                _scheduler.Start();
        }

        ~SchedulerService()
        {
            if (!_scheduler.IsShutdown)
                _scheduler.Shutdown();
        }

        public void Dispose()
        {
            if (!_scheduler.IsShutdown)
                _scheduler.Shutdown();
        }

        public void AddJob(IJobDetail job, ITrigger trigger)
        {
            job.JobDataMap.Add(nameof(IServiceProvider), _serviceProvider);
            Task.WaitAll(_scheduler.DeleteJob(job.Key));
            Task.WaitAll(_scheduler.ScheduleJob(job, trigger));
        }

        public void RemoveJob(JobKey jobkey)
        {
            _scheduler.DeleteJob(jobkey);
        }
    }
}
