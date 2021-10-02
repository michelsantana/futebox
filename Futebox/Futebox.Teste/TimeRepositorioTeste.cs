using Futebox.DB;
using Futebox.Models;
using Xunit;

namespace Futebox.Teste
{
    public class TimeRepositorioTeste
    {
        DatabaseConfig dbConfig;

        public TimeRepositorioTeste()
        {
            dbConfig = new DatabaseConfig("Data Source=D:/Notebook/Workspace/pessoal/FuteBox/core/Futebox/Futebox/DB/futebox.sqlite");
        }

        [Fact]
        public void DeveSalvarUmNovoTime()
        {
            RegisterMappings.Register();
            TimeRepositorio repository = new TimeRepositorio(dbConfig);

            var time = new Time()
            {
                nome = "TimeDeTeste",
                origemDado = "Teste",
                origem_ext_id = "0",
                origem_ext_equipe_id = "0"
            };
            repository.Insert(ref time);
            Assert.NotNull(time.id);
        }

        [Fact]
        public void DeveRemoverUmTimeSalvo()
        {
            RegisterMappings.Register();
            TimeRepositorio repository = new TimeRepositorio(dbConfig);

            var time = new Time()
            {
                nome = "TimeDeTeste",
                origemDado = "Teste",
                origem_ext_id = "0",
                origem_ext_equipe_id = "0"
            };
            repository.Insert(ref time);
            var deleted = repository.Delete(time.id);
            var result = repository.GetById(time.id);

            Assert.True(deleted);
            Assert.Null(result?.id);
        }
    }
}
