using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Futebox.Teste
{
    [Collection(nameof(DataBaseCollection))]
    public class FootstatsTeste
    { 
        [Fact]
        public void ObterTimesDoServico_DeveSerValido_QuandoListaTiverItem()
        {
            // Arrange
            var mocker = new AutoMocker();
            var _service = mocker.CreateInstance<FootstatsService>();
            var httpMock = mocker.GetMock<IHttpHandler>();
            var campeonatosAtivos = CampeonatoUtils.ObterCampeonatosAtivos();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{data:[{}]}"),
            };

            httpMock.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(response);

            // Act
            var result = _service.ObterTimesServico();

            // Assert
            httpMock.Verify(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(campeonatosAtivos.Length));
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("[]", 0)]
        [InlineData("{}", 0)]
        [InlineData("{data:{}}", 0)]
        [InlineData("[{},{}]", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void ObterTimesDoServico_DeveSerValido_MesmoComRetornoInconsistenteDaApi(string jsonRetornadoApi, int quantidadeRegistrosEsperados)
        {
            // Arrange
            var mocker = new AutoMocker();
            var _service = mocker.CreateInstance<FootstatsService>();
            var httpMock = mocker.GetMock<IHttpHandler>();
            var campeonatosAtivos = CampeonatoUtils.ObterCampeonatosAtivos();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = jsonRetornadoApi == null ? null : new StringContent(jsonRetornadoApi),
            };

            httpMock.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(response);

            // Act
            var result = _service.ObterTimesServico();

            // Assert
            httpMock.Verify(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(campeonatosAtivos.Length));
            Assert.NotNull(result);
            Assert.Equal(quantidadeRegistrosEsperados, result.Count);
        }

        /// <summary>
        /// O teste ObterTimesDoServico_DeveSerValido_QuandoRetornarDadosDaApi acessa a API verdadeira
        /// </summary>
        [Fact]
        public void ObterTimesDoServico_DeveSerValido_QuandoRetornarDadosDaApi()
        {
            // Arrange
            var _http = new HttpHandler();
            var _logger = new Mock<ILogger<IFootstatsService>>();
            var _service = new FootstatsService(_http, _logger.Object);

            //Act
            var resultado = _service.ObterTimesServico();

            // Assert
            Assert.NotEmpty(resultado);
        }

        /// <summary>
        /// O teste ObterClassificacaoServico_DeveSerValido_QuandoRetornarDadosDaApi acessa a API verdadeira
        /// </summary>
        [Fact]
        public void ObterClassificacaoServico_DeveSerValido_QuandoRetornarDadosDaApi()
        {
            // Arrange
            var _http = new HttpHandler();
            var _logger = new Mock<ILogger<IFootstatsService>>();
            var _service = new FootstatsService(_http, _logger.Object);

            //Act
            var resultado = _service.ObterClassificacaoServico(CampeonatoUtils.ObterCampeonatosAtivos()[0]);

            // Assert
            Assert.NotEmpty(resultado);
        }
    }
}
