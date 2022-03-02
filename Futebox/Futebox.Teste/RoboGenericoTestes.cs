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
    public class RoboGenericoTestes
    {
        private readonly InjectorFixture _injector;

        private readonly ProcessoRepositorio _processoRepository;
        private readonly SubProcessoRepositorio _subProcessoRepository;

        public RoboGenericoTestes(InjectorFixture injector)
        {
            _injector = injector;
            _injector.DefinirSettings();

            RegisterMappings.Register();

            var dbconfig = _injector.GerarConfiguracaoDoBancoDeDados();
            var ytv = new RepositoryBase<SubProcessoYoutubeVideo>(dbconfig);
            var yts = new RepositoryBase<SubProcessoYoutubeShort>(dbconfig);
            var igv = new RepositoryBase<SubProcessoInstagramVideo>(dbconfig);

            _subProcessoRepository = new SubProcessoRepositorio(ytv, yts, igv);
            _processoRepository = new ProcessoRepositorio(dbconfig);
        }

        private SubProcesso GerarUmSubProcesso()
        {
#warning TODO: MANUALLY 
            var s = (_subProcessoRepository.GetById("TESTEIG")).FirstOrDefault();
            s.linkDaImagemDoVideo = s.linkDaImagemDoVideo.Replace("51203", "5001");
            s.linkPostagem = s.linkPostagem?.Replace("51203", "5001");
            return s;
        }

        [Fact]
        public async Task ExtrairAudioDoProcesso_Cache()
        {
            var _browserService = new BrowserService();
            var _futebotService = new FutebotService(_browserService);
            var subProcessoIG = GerarUmSubProcesso();
            var file = Path.Combine(subProcessoIG.pastaDoArquivo, subProcessoIG.nomeDoArquivoAudio);

            if (!Directory.Exists(subProcessoIG.pastaDoArquivo)) Directory.CreateDirectory(subProcessoIG.pastaDoArquivo);
            if (File.Exists(file)) File.Delete(file);

            var result = await _futebotService.GerarAudio(subProcessoIG, true);

            Assert.True(result.IsOk());
            Assert.Contains(result.stack, _ => _.Contains("cache"));
            Assert.True(File.Exists(file));
        }

        [Fact]
        public async Task ExtrairAudioDoProcesso_SemCache()
        {
            var _browserService = new BrowserService();
            var _futebotService = new FutebotService(_browserService);
            var subProcessoIG = GerarUmSubProcesso();
            var file = Path.Combine(subProcessoIG.pastaDoArquivo, subProcessoIG.nomeDoArquivoAudio);

            if (!Directory.Exists(subProcessoIG.pastaDoArquivo)) Directory.CreateDirectory(subProcessoIG.pastaDoArquivo);
            if (File.Exists(file)) File.Delete(file);

            var result = await _futebotService.GerarAudio(subProcessoIG, false);

            Assert.True(result.IsOk());
            Assert.DoesNotContain(result.stack, _ => _.Contains("cache"));
            Assert.True(File.Exists(file));
        }
    }
}
