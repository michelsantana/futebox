using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IQueueService
    {
        public Task Executar(Func<Task> task);
    }
}
