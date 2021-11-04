using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class SchedulerService : IDisposable
    {
        static StdSchedulerFactory _factory;
        static IScheduler _scheduler;

        public SchedulerService()
        {
            _factory = _factory ?? new StdSchedulerFactory();
            _scheduler = _scheduler ?? _factory.GetScheduler().Result;
            if (!_scheduler.IsStarted)
                _scheduler.Start();
        }

        ~SchedulerService()
        {
            if(!_scheduler.IsShutdown)
                _scheduler.Shutdown();
        }

        public void Dispose()
        {
            if (!_scheduler.IsShutdown)
                _scheduler.Shutdown();
        }

        public void AddJob(IJobDetail job, ITrigger trigger)
        {
            Task.WaitAll(_scheduler.DeleteJob(job.Key));
            Task.WaitAll(_scheduler.ScheduleJob(job, trigger));
        }
    }
}
