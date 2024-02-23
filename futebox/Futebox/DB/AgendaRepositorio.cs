using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Futebox.Models;
using System;

namespace Futebox.DB
{
    public class AgendaRepositorio : RepositoryBase<Agenda>, IAgendaRepositorio
    {
        public AgendaRepositorio(IDatabaseConfig dbConfig) : base(dbConfig)
        {

        }

        public AgendaRepositorio() : base(null)
        {

        }
    }
}
