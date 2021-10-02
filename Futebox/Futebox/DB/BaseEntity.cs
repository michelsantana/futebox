using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB
{
    public class BaseEntity
    {
        public string id { get; set; }
        public DateTime criacao { get; set; }
        public DateTime? alteracao { get; set; }
    }
}
