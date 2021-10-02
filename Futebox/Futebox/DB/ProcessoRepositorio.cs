using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Futebox.Models;

namespace Futebox.DB
{
    public class ProcessoRepositorio : RepositoryBase<Processo>, IProcessoRepositorio
    {
        public ProcessoRepositorio(IDatabaseConfig dbConfig) : base(dbConfig)
        {

        }

        public ProcessoRepositorio() : base(null)
        {

        }
    }
}
