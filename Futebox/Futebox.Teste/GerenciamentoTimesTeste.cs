using Futebox.DB;
using Futebox.Models;
using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Futebox.Teste
{
    [Collection(nameof(DataBaseCollection))]
    public class GerenciamentoTimesTeste
    {

        private readonly DatabaseFixture _db;

        public GerenciamentoTimesTeste(DatabaseFixture db)
        {
            _db = db;
        }
        private FootstatsTime GerarUmTimeDoFootstats()
        {
            var retorno = new FootstatsTime()
            {
                urlLogo = "https://frontendapiapp.blob.core.windows.net/images/88x88/america-mg.png",
                nome = "América-MG",
                idTecnico = 54,
                tecnico = "Vágner Mancini",
                cidade = "Belo Horizonte",
                estado = "MG",
                pais = "Brasil",
                hasScout = true,
                sde = new FootstatsTime.Sde() { equipe_id = 327 },
                isTimeGrande = false,
                selecao = false,
                torcedorNoSingular = "america",
                torcedorNoPlural = "americas",
                timeFantasia = false,
                grupo = null,
                sigla = "AMG",
                id = 1085
            };
            return retorno;
        }


        [Fact]
        public void SalvarTime_DeveSalvarTime_TerBrasao_RemoverTimeSalvo()
        {
            var footstatsTime = GerarUmTimeDoFootstats();
            footstatsTime.id = 9990;

            var _cache = new Mock<ICacheHandler>();
            var _logger = new Mock<ILogger<IGerenciamentoTimesService>>();
            var _footstatsService = new Mock<IFootstatsService>();

            var _timeRepositorio = new TimeRepositorio(_db.GerarConfiguracaoDoBancoDeDados());
            var _service = new GerenciamentoTimesService(_cache.Object, _timeRepositorio, _footstatsService.Object, _logger.Object);

            var time = _service.SalvarTime(footstatsTime);
            Assert.NotNull(time?.id);
            Assert.NotNull(time?.logoBin);

            var id = time?.id;

            var selected = _timeRepositorio.GetById(id);
            Assert.NotNull(selected?.id);

            var deleted = _service.RemoverTimeDB(id);
            Assert.True(deleted);

            selected = _timeRepositorio.GetById(time?.id);
            Assert.Null(selected?.id);
        }
    }
}
