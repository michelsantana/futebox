using Futebox.DB;
using Futebox.Models;
using Futebox.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Futebox.Teste
{
    [Collection(nameof(InjectorCollection))]
    public class BrowserTestes
    {
        private readonly InjectorFixture _injector;

        public BrowserTestes(InjectorFixture injector)
        {
            _injector = injector;
            _injector.DefinirSettings();
            RegisterMappings.Register();
        }

        [Fact]
        public async Task Conectar_Browser()
        {
            // Arrange
            var _browserService = new BrowserService();

            var page = await _browserService.NewPage();

            Assert.NotNull(page);

            await page.CloseAsync();
        }
    }
}
