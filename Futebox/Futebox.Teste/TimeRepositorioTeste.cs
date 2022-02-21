using Futebox.DB;
using Futebox.Models;
using Xunit;

namespace Futebox.Teste
{
    [Collection(nameof(DataBaseCollection))]
    public class TimeRepositorioTeste
    {
        private readonly DatabaseFixture _db;

        public TimeRepositorioTeste(DatabaseFixture db)
        {
            RegisterMappings.Register();
            _db = db;
        }

        [Fact]
        public void DeveSalvarUmNovoTime()
        {
            TimeRepositorio repository = new TimeRepositorio(_db.GerarConfiguracaoDoBancoDeDados());

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
            TimeRepositorio repository = new TimeRepositorio(_db.GerarConfiguracaoDoBancoDeDados());

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
