using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Futebox.Models;
using System;

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

        public override string GenerateID()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss-ff");
        }
    }
}
