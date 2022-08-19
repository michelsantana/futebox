using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class QueueService : IQueueService
    {
        private SemaphoreSlim semaphore;

        public QueueService()
        {
            semaphore = new SemaphoreSlim(1);
        }

        public async Task Executar(Func<object[], Task> task, object[] args)
        {
            await semaphore.WaitAsync();
            try
            {
                await task(args);
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task Executar(Func<Task> task)
        {
            await Executar(async (o) => await task(), null);
        }
    }
}
