using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class RabbitMessage
    {
        public string eventName { get; set; }
        public string payload { get; set; }
    }
}
