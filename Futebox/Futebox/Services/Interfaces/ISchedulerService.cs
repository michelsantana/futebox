using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface ISchedulerService : IDisposable
    {
        void AddJob(IJobDetail job, ITrigger trigger);
        void RemoveJob(string jobkey);
        IJobDetail Info(string jobKey);
        Task<List<IJobDetail>> List();
        Task<List<IJobExecutionContext>> ListRunning();
    }
}
