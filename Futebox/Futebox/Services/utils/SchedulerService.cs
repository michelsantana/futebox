using Futebox.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
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
        
        JobKey Key(string key) => new JobKey(key, "geral");

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

        public async void AddJob(IJobDetail job, ITrigger trigger)
        {
            job.JobDataMap.Add(nameof(IServiceProvider), _serviceProvider);
            Task.WaitAll(_scheduler.DeleteJob(job.Key));
            Task.WaitAll(_scheduler.ScheduleJob(job, trigger));
        }

        public async void RemoveJob(string jobkey)
        {
            await _scheduler.DeleteJob(Key(jobkey));
        }

        public IJobDetail Info(string jobKey)
        {
            return _scheduler.GetJobDetail(Key(jobKey)).Result;
        }

        public async Task<List<IJobDetail>> List()
        {
            var list = new List<IJobDetail>();
            var groups = await _scheduler.GetJobGroupNames();
            foreach (String groupName in groups)
            {
                var keys =
                    await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
                foreach (JobKey jobKey in keys)
                {
                    list.Add(await _scheduler.GetJobDetail(jobKey));
                }
            }
            return list;
        }

        public async Task<List<IJobExecutionContext>> ListRunning()
        {
            return (await _scheduler.GetCurrentlyExecutingJobs()).ToList();
        }

        public async Task Cancelar(string key)
        {
            await _scheduler.DeleteJob(Key(key));
        }
    }
}
