using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Futebox.Models;

namespace Futebox.DB
{
    public class TimeRepositorio : RepositoryBase<Time>, ITimeRepositorio
    {
        public TimeRepositorio(IDatabaseConfig dbConfig) : base(dbConfig)
        {

        }

        public TimeRepositorio() : base(null)
        {

        }
    }
}
