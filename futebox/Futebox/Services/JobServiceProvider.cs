using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public abstract class JobServiceProvider : IJob
    {
        protected IServiceProvider _serviceProvider;

        protected void LoadServiceProviderFromContext(IJobExecutionContext context)
        {
            this._serviceProvider = (IServiceProvider)context.MergedJobDataMap[nameof(IServiceProvider)];
        }

        protected T GetService<T>()
        {
            return (T)this._serviceProvider.GetService(typeof(T));
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            LoadServiceProviderFromContext(context);
        }

        public static JobDataMap JobData(IServiceProvider serviceProvider)
        {
            IDictionary<string, object> jobData = new Dictionary<string, object>();
            jobData.Add(nameof(IServiceProvider), serviceProvider);
            return new JobDataMap(jobData);
        }
    }
}
