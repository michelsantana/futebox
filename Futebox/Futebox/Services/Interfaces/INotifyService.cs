using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface INotifyService
    {
        Task Notify(string message);
    }
}
