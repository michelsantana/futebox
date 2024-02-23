using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models.Viewmodels
{
    public class AgendaVM : Agenda
    {
        public bool hasJob { get; set; }
        public string jobSchedulerName { get; set; }
        public bool jobIsRunning { get; set; }
    }
}
