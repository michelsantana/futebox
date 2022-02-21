using Futebox.DB;
using Futebox.Models;
using Futebox.Services;
using Moq.AutoMock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Futebox.Teste
{
    [Collection(nameof(DataBaseCollection))]
    public class RoboTestes
    {
        private readonly DatabaseFixture _db;
        private readonly ProcessoRepositorio _processoRepository;
        private readonly SubProcessoRepositorio _subProcessoRepository;

        public RoboTestes(DatabaseFixture db)
        {
            RegisterMappings.Register();
            _db = db;
            var dbconfig = _db.GerarConfiguracaoDoBancoDeDados();
            _db.DefinirSettings();
            var ytv = new RepositoryBase<SubProcessoYoutubeVideo>(dbconfig);
            var yts = new RepositoryBase<SubProcessoYoutubeShort>(dbconfig);
            var igv = new RepositoryBase<SubProcessoInstagramVideo>(dbconfig);
            _subProcessoRepository = new SubProcessoRepositorio(ytv, yts, igv);
            _processoRepository = new ProcessoRepositorio(dbconfig);
        }

        private SubProcesso GerarUmSubProcessoIG()
        {
            return (_subProcessoRepository.GetById("TESTEIG")).FirstOrDefault();
        }

        private SubProcesso GerarUmSubProcessoYT()
        {
            return (_subProcessoRepository.GetById("TESTEYT")).FirstOrDefault();
        }


        Process proc;
        private void StartupServer()
        {
            proc = new Process();
            proc.StartInfo.FileName = $"{Path.Combine(Settings.ApplicationsRoot, "handle-server-for-tests")}";
            proc.StartInfo.WorkingDirectory = Settings.ApplicationsRoot;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void KillServer()
        {
            try
            {
                proc.Kill(true);
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
            }
        }

        [Fact]
        public async Task Conectar_Browser()
        {
            // Arrange
            var _browserService = new BrowserService();

            var page = await _browserService.NewPage();

            Assert.NotNull(page);
        }

        [Fact]
        public async Task ExtrairImagemProcesso()
        {
            // dotnet run --urls=http://localhost:5001/
            var _browserService = new BrowserService();
            var _futebotService = new FutebotService(_browserService);

            var subProcessoIG = GerarUmSubProcessoIG();
            subProcessoIG.linkDaImagemDoVideo = subProcessoIG.linkDaImagemDoVideo.Replace("51203", "5001");
            subProcessoIG.linkPostagem = subProcessoIG.linkPostagem?.Replace("51203", "5001");

            var file = Path.Combine(subProcessoIG.pastaDoArquivo, subProcessoIG.nomeDoArquivoImagem);

            if (!Directory.Exists(subProcessoIG.pastaDoArquivo)) Directory.CreateDirectory(subProcessoIG.pastaDoArquivo);
            if (File.Exists(file)) File.Delete(file);

            var result = await _futebotService.GerarImagem(subProcessoIG);

            Assert.True(result.IsOk());
            Assert.True(File.Exists(Path.Combine(subProcessoIG.pastaDoArquivo, subProcessoIG.nomeDoArquivoImagem)));
        }

        [Fact]
        public async Task ExtrairAudioDoProcesso_Cache()
        {
            StartupServer();



            KillServer();
        }


        [Fact]
        public async Task RealizarUploadParaInstagram()
        {
            _db.DefinirSettings();

            var _browserService = new BrowserService();
            var _instagramService = new InstagramService(_browserService);

            var subProcesso = GerarUmSubProcessoIG();

            var result = await _instagramService.Upload(subProcesso);

            Assert.True(result.IsOk());
        }

        [Fact]
        public async Task RealizarUploadParaYoutube()
        {
            _db.DefinirSettings();

            var _browserService = new BrowserService();
            var _youtubeService = new YoutubeService(_browserService);

            var subProcesso = GerarUmSubProcessoYT();

            var result = await _youtubeService.Upload(subProcesso);

            Assert.True(result.IsOk());
        }

    }
}
