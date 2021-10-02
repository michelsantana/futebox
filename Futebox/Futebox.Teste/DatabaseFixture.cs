using Futebox.DB;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Futebox.Teste
{

    [CollectionDefinition(nameof(DataBaseCollection))]
    public class DataBaseCollection : ICollectionFixture<DatabaseFixture>
    { }

    public class DatabaseFixture : IDisposable
    {
        public DatabaseConfig GerarConfiguracaoDoBancoDeDados()
        {
            var db = new DatabaseConfig("Data Source=D:/Notebook/Workspace/pessoal/FuteBox/core/Futebox/Futebox/DB/futebox.sqlite");
            return db;
        }
        public void Dispose()
        {
        }
    }
}
