using Futebox.DB;
using Futebox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Futebox.Teste
{
    [Collection(nameof(InjectorCollection))]
    public class FluxoProcesso
    {

        private readonly InjectorFixture _injector;
        private readonly ProcessoRepositorio _processoRepository;
        //private readonly SubProcessoRepositorio _subProcessoRepository;

        public FluxoProcesso(InjectorFixture injector)
        {
            _injector = injector;
            _injector.DefinirSettings();

            RegisterMappings.Register();

            var dbconfig = _injector.GerarConfiguracaoDoBancoDeDados();
            //var ytv = new RepositoryBase<SubProcessoYoutubeVideo>(dbconfig);
            //var yts = new RepositoryBase<SubProcessoYoutubeShort>(dbconfig);
            //var igv = new RepositoryBase<SubProcessoInstagramVideo>(dbconfig);

            _subProcessoRepository = new SubProcessoRepositorio(ytv, yts, igv);
            _processoRepository = new ProcessoRepositorio(dbconfig);
        }

        [Fact]
        public async Task CriarUmProcessoClassificacao()
        {



            //var p = new Processo()
            //{
            //    id = "test-case-gen",
            //    nome = "Processo de testes",
            //    criacao = DateTime.Now,
            //    agendado = false,
            //    agendamento = null,
            //    alteracao = null,
            //    campeonatoId = $"{(int)CampeonatoUtils.ObterCampeonatosAtivos().First()}",
            //    categoria = Models.Enums.CategoriaVideo.partida,
            //    partidaId = "-9000",
            //    pastaDosArquivos = Path.Combine(Settings.ApplicationsRoot, "test-case-gen"),
            //    rodadaId = "-1",
            //    status = Models.Enums.StatusProcesso.Criado,
            //    statusMensagem = "",
            //    notificacao = ""
            //};
            //_processoRepository.Insert(p);


            //var sub = SubProcessoYoutubeShort.New(processo.id,
            //    CategoriaVideo.partida,
            //    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=yts",
            //    roteiro,
            //    atributos.Item1,
            //    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
            //    "short");
            //_subProcessoRepositorio.Inserir(sub);

            //var sub = SubProcessoYoutubeVideo.New(processo.id,
            //    CategoriaVideo.partida,
            //    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=ytv",
            //    roteiro,
            //    atributos.Item1,
            //    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
            //    "short");
            //_subProcessoRepositorio.Inserir(sub);

            //var sub = SubProcessoInstagramVideo.New(processo.id,
            //    CategoriaVideo.partida,
            //    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=igv",
            //    roteiro,
            //    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}");
            //_subProcessoRepositorio.Inserir(sub);

        }
    }
}
