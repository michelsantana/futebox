using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Interfaces
{
    public interface IDatabaseConfig
    {
        string ConnectionString();
    }
}
