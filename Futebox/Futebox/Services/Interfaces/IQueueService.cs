using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IQueueService
    {
        Task Executar(Func<Task> task);
        Task Executar(Func<object[], Task> task, object[] args);
    }
}
