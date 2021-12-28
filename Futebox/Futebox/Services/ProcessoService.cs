using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Futebox.Models.Enums;
using System.Threading.Tasks;
using System.Net;

namespace Futebox.Services
{
    public class ProcessoService : IProcessoService
    {
        IProcessoRepositorio _processoRepositorio;
        ISubProcessoRepositorio _subProcessoRepositorio;

        IPartidasService _partidasService;
        IClassificacaoService _classificacaoService;
        IRodadaService _rodadaService;
        IFutebotService _futebotService;

        public ProcessoService(IProcessoRepositorio processoRepositorio, ISubProcessoRepositorio subProcessoRepositorio,
        IPartidasService partidasService, IClassificacaoService classificacaoService, IRodadaService rodadaService, IFutebotService futebotService)
        {
            _processoRepositorio = processoRepositorio;
            _subProcessoRepositorio = subProcessoRepositorio;
            _partidasService = partidasService;
            _classificacaoService = classificacaoService;
            _rodadaService = rodadaService;
            _futebotService = futebotService;
        }

        public List<Processo> ObterProcessos()
        {
            return _processoRepositorio.GetAll()?.ToList();
        }

        public Processo ObterProcesso(string id)
        {
            return _processoRepositorio.GetSingle(_ => _.id == id);
        }

        public Processo SalvarProcessoPartida(int idPartida)
        {
            var partidas = _partidasService.ObterPartida(idPartida);

            return SalvarProcessoPartida(partidas);

        }
        private Processo SalvarProcessoPartida(PartidaVM partida)
        {
            var atributos = _partidasService.ObterAtributosDoVideo(partida);
            var processo = new Processo()
            {
                nome = $"{atributos.Item1}",
                categoria = CategoriaVideo.partida,
                partidaId = partida.idExterno.ToString(),
                campeonatoId = partida.campeonato,
                rodadaId = partida.rodada.ToString(),
                status = StatusProcesso.Criado,
                agendado = false,
                agendamento = partida.dataPartida.AddHours(2).AddMinutes(10),
                notificacao =
                $"processo: partida" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            foreach (var redeSocial in partida.sociais)
            {
                SalvarSubProcessoPartida(partida, atributos, processo, redeSocial);
            }
            return processo;
        }
        private void SalvarSubProcessoPartida(PartidaVM partida, Tuple<string, string> atributos, Processo processo, RedeSocialFinalidade redeSocial)
        {
            var roteiro = _partidasService.ObterRoteiroDaPartida(partida);

            if (redeSocial == RedeSocialFinalidade.YoutubeShorts)
            {
                var sub = SubProcessoYoutubeShort.New(processo.id,
                    CategoriaVideo.partida,
                    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=yts",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "short");
                _subProcessoRepositorio.Inserir(sub);
            }
            if (redeSocial == RedeSocialFinalidade.YoutubeVideo)
            {
                var sub = SubProcessoYoutubeVideo.New(processo.id,
                    CategoriaVideo.partida,
                    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=ytv",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "short");
                _subProcessoRepositorio.Inserir(sub);
            }
            if (redeSocial == RedeSocialFinalidade.InstagramVideo)
            {
                var sub = SubProcessoInstagramVideo.New(processo.id,
                    CategoriaVideo.partida,
                    $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=igv",
                    roteiro,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}");
                _subProcessoRepositorio.Inserir(sub);
            }
        }

        public Processo SalvarProcessoClassificacao(Campeonatos campeonato)
        {
            var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(campeonato, true);
            return SalvarProcessoClassificacao(new RedeSocialFinalidade[] { RedeSocialFinalidade.YoutubeVideo }.ToList(), classificacao, campeonato);
        }
        private Processo SalvarProcessoClassificacao(List<RedeSocialFinalidade> redesSociais, IEnumerable<ClassificacaoVM> classificacao, Campeonatos campeonato)
        {
            var atributos = _classificacaoService.ObterAtributosDoVideo(classificacao, campeonato);
            var processo = new Processo()
            {
                nome = $"{atributos.Item1}",
                categoria = CategoriaVideo.partida,
                partidaId = null,
                campeonatoId = ((int)campeonato).ToString(),
                rodadaId = null,
                status = StatusProcesso.Criado,
                agendado = false,
                agendamento = DateTime.Today.AddHours(23).AddMinutes(30),
                notificacao =
                $"processo: classificacao" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            foreach (var redeSocial in redesSociais)
            {
                SalvarSubProcessoClassificacao(classificacao, atributos, processo, campeonato, redeSocial);
            }
            return processo;
        }
        private void SalvarSubProcessoClassificacao(IEnumerable<ClassificacaoVM> classificacao, Tuple<string, string> atributos, Processo processo, Campeonatos campeonato, RedeSocialFinalidade redeSocial)
        {
            var roteiro = _classificacaoService.ObterRoteiroDaClassificacao(classificacao, campeonato);
            if (redeSocial == RedeSocialFinalidade.YoutubeVideo)
            {
                var sub = SubProcessoYoutubeVideo.New(processo.id,
                    CategoriaVideo.classificacao,
                    $"{Settings.ApplicationHttpBaseUrl}classificacao?campeonato={(int)campeonato}&viewMode=ytv",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "classificacao");
                _subProcessoRepositorio.Inserir(sub);
            }

            if (redeSocial == RedeSocialFinalidade.YoutubeShorts)
            {
                var sub = SubProcessoYoutubeShort.New(processo.id,
                    CategoriaVideo.classificacao,
                    $"{Settings.ApplicationHttpBaseUrl}classificacao?campeonato={(int)campeonato}&viewMode=yts",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "classificacao");
                _subProcessoRepositorio.Inserir(sub);
            }

            if (redeSocial == RedeSocialFinalidade.YoutubeVideo)
            {
                var sub = SubProcessoInstagramVideo.New(processo.id,
                    CategoriaVideo.classificacao,
                    $"{Settings.ApplicationHttpBaseUrl}classificacao?campeonato={(int)campeonato}&viewMode=igv",
                    roteiro,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}");
                _subProcessoRepositorio.Inserir(sub);
            }
        }

        public Processo SalvarProcessoRodada(Campeonatos campeonato, int rodada)
        {
            var partidas = _rodadaService.ObterPartidasDaRodada(campeonato, rodada);
            return SalvarProcessoRodada(new RedeSocialFinalidade[] { RedeSocialFinalidade.YoutubeVideo }.ToList(), partidas, campeonato, rodada);

        }
        public Processo SalvarProcessoRodada(List<RedeSocialFinalidade> redesSociais, IEnumerable<PartidaVM> partidas, Campeonatos campeonato, int rodada)
        {
            var atributos = _rodadaService.ObterAtributosDoVideo(partidas, campeonato, rodada);
            var processo = new Processo()
            {
                nome = $"{atributos.Item1}",
                categoria = CategoriaVideo.partida,
                partidaId = null,
                campeonatoId = ((int)campeonato).ToString(),
                rodadaId = rodada.ToString(),
                status = StatusProcesso.Criado,
                agendado = false,
                agendamento = DateTime.Now.AddMinutes(30),
                notificacao =
                $"processo: rodada" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            foreach (var redeSocial in redesSociais)
            {
                SalvarSubProcessoRodada(partidas, rodada, atributos, processo, campeonato, redeSocial);
            }
            return processo;
        }
        private void SalvarSubProcessoRodada(IEnumerable<PartidaVM> partidas, int rodada, Tuple<string, string> atributos, Processo processo, Campeonatos campeonato, RedeSocialFinalidade redeSocial)
        {
            var roteiro = _rodadaService.ObterRoteiroDaRodada(partidas, campeonato, rodada);
            if (redeSocial == RedeSocialFinalidade.YoutubeVideo)
            {
                var sub = SubProcessoYoutubeVideo.New(processo.id,
                    CategoriaVideo.rodada,
                    $"{Settings.ApplicationHttpBaseUrl}rodadas?campeonato={(int)campeonato}&rodada={rodada}&viewMode=ytv",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "rodada");
                _subProcessoRepositorio.Inserir(sub);
            }

            if (redeSocial == RedeSocialFinalidade.YoutubeShorts)
            {
                var sub = SubProcessoYoutubeShort.New(processo.id,
                    CategoriaVideo.rodada,
                    $"{Settings.ApplicationHttpBaseUrl}rodadas?campeonato={(int)campeonato}&rodada={rodada}&viewMode=yts",
                    roteiro,
                    atributos.Item1,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}",
                    "rodada");
                _subProcessoRepositorio.Inserir(sub);
            }

            if (redeSocial == RedeSocialFinalidade.YoutubeVideo)
            {
                var sub = SubProcessoInstagramVideo.New(processo.id,
                    CategoriaVideo.rodada,
                    $"{Settings.ApplicationHttpBaseUrl}rodadas?campeonato={(int)campeonato}&rodada={rodada}&viewMode=igv",
                    roteiro,
                    atributos.Item2 + $"\r\n{ObterDescricaoDefault()}");
                _subProcessoRepositorio.Inserir(sub);
            }
        }

        public Processo AgendarProcesso(string id, DateTime hora)
        {
            var p = _processoRepositorio.GetById(id);
            p.agendamento = hora;
            p.status = StatusProcesso.Agendado;
            p.agendado = true;

            _processoRepositorio.OpenTransaction();
            _processoRepositorio.Update(p);
            _processoRepositorio.Commit();

            var subs = ObterSubProcessos(id);
            subs.ForEach(_ =>
            {
                if (_.status == StatusProcesso.Criado)
                {
                    _.status = StatusProcesso.Agendado;
                    _.alteracao = DateTime.Now;
                    _subProcessoRepositorio.Update(_);
                }
            });

            return p;
        }

        public void GerarImagem(ref Processo processo, ref SubProcesso sub)
        {
            //AtualizarRoteiro(processo);
            var resultado = _futebotService.GerarImagem(sub);
            AtualizarProcessoLog(processo.id, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.ImagemOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.ImagemErro);
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.Erro);
            }
        }

        public void GerarAudio(ref Processo processo, ref SubProcesso sub)
        {
            //AtualizarRoteiro(processo);
            AtualizarStatus(ref processo, ref sub, StatusProcesso.GerandoImagem);
            var resultado = _futebotService.GerarAudio(sub);

            AtualizarProcessoLog(processo.id, resultado.stack.ToArray());
            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.AudioOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.AudioErro);
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.Erro);
            }
            //return processoRetorno;
        }

        public void GerarVideo(ref Processo processo, ref SubProcesso sub)
        {
            //AtualizarRoteiro(processo);
            AtualizarStatus(ref processo, ref sub, StatusProcesso.GerandoAudio);
            var resultado = _futebotService.GerarVideo(sub);

            AtualizarProcessoLog(processo.id, resultado.stack.ToArray());
            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.VideoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.VideoErro);
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.Erro);
            }
            //return processoRetorno;
        }

        public void PublicarVideo(ref Processo processo, ref SubProcesso sub)
        {
            //AtualizarRoteiro(processo);
            AtualizarStatus(ref processo, ref sub, StatusProcesso.GerandoVideo);
            var resultado = _futebotService.PublicarVideo(sub);

            AtualizarProcessoLog(processo.id, resultado.stack.ToArray());
            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.PublicandoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.PublicandoErro);
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.Erro);
            }
            if (resultado.status == HttpStatusCode.Unauthorized)
            {
                AtualizarStatus(ref processo, ref sub, StatusProcesso.PublicandoErro);
            }
            //return processoRetorno;
        }

        public void AbrirPasta(Processo processo)
        {
            var resultado = _futebotService.AbrirPasta(processo);
            if (resultado.status == HttpStatusCode.InternalServerError) throw new Exception("Erro ao gerar o vídeo!");
            if (resultado.status == HttpStatusCode.BadRequest) throw new Exception("Comando inválido");
        }

        public void AtualizarStatus(ref Processo processo, ref SubProcesso sub, StatusProcesso status)
        {
            //var p = _processoRepositorio.GetById(id);
            processo.alteracao = DateTime.Now;
            processo.status = StatusProcesso.Agendado;
            _processoRepositorio.Update(processo);

            sub.status = status;
            sub.alteracao = DateTime.Now;
            sub = _subProcessoRepositorio.Update(sub);
        }

        public void AtualizarProcessoLog(string id, string[] lines)
        {
            var p = _processoRepositorio.GetById(id);
            p.alteracao = DateTime.Now;
            p.statusMensagem = $"{p.statusMensagem}\n[UPDATE:{p.alteracao}]\n{string.Join("\n", lines)}";

            _processoRepositorio.Update(p);
        }

        public Processo AtualizarRoteiro(Processo processo)
        {
            switch (processo.categoria)
            {
                case CategoriaVideo.partida:
                    return AtualizarRoteiroPartida(processo);
                case CategoriaVideo.classificacao:
                    return AtualizarRoteiroClassificacao(processo);
                case CategoriaVideo.rodada:
                    return AtualizarRoteiroRodada(processo);
            }
            return processo;
        }

        public bool Delete(string id)
        {
            _processoRepositorio.Delete(id);
            return true;
        }

        private Processo AtualizarRoteiro(string processo)
        {
            return AtualizarRoteiro(ObterProcesso(processo));
        }

        private Processo AtualizarRoteiroPartida(Processo processo)
        {
            //var args = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(processo.args);
            //processo.roteiro = _partidasService.ObterRoteiroDaPartida(args.partidaId);
            //_processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroClassificacao(Processo processo)
        {
            //var args = JsonConvert.DeserializeObject<ProcessoClassificacaoArgs>(processo.args);
            //var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(args.campeonato, true);
            //processo.roteiro = _classificacaoService.ObterRoteiroDaClassificacao(classificacao, args.campeonato);
            //_processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroRodada(Processo processo)
        {
            //var args = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(processo.args);
            //var partidas = _rodadaService.ObterPartidasDaRodada(args.campeonato, args.rodada, true);
            //processo.roteiro = _rodadaService.ObterRoteiroDaRodada(partidas, args.campeonato, args.rodada);
            //_processoRepositorio.Update(processo);
            return processo;
        }

        private string ObterDescricaoDefault()
        {
            var arr = new string[] {
                "BRASILEIRÃO", "BRASILEIRÃO 2021",
                "BRASILEIRAO", "BRASILEIRAO 2021", "#BRASILEIRAO", "#BRASILEIRAO2021",
                "TABELA BRASILEIRÃO", "TABELA BRASILEIRÃO 2021", "TABELA BRASILEIRAO",
                "TABELA BRASILEIRAO 2021", "#TABELABRASILEIRAO", "#TABELABRASILEIRAO2021",
                "TABELA CLASSIFICAÇÃO", "TABELA CLASSIFICAÇÃO 2021", "TABELA CLASSIFICACAO",
                "TABELA CLASSIFICACAO 2021", "#TABELACLASSIFICACAO", "#TABELACLASSIFICACAO2021",
                "CLASSIFICAÇÃO DO BRASILEIRÃO","CLASSIFICAÇÃO DO BRASILEIRÃO 2021", "CLASSIFICACAO DO BRASILEIRAO",
                "CLASSIFICACAO DO BRASILEIRAO 2021","#CLASSIFICACAODOBRASILEIRAO", "#CLASSIFICACAODOBRASILEIRAO2021",
                "CAMPEONATO BRASILEIRO", "CAMPEONATO BRASILEIRO 2021", "CAMPEONATO BRASILEIRO",
                "CAMPEONATO BRASILEIRO 2021","#CAMPEONATOBRASILEIRO", "#CAMPEONATOBRASILEIRO2021"
            };
            return string.Join("\r\n", arr);
        }

        public List<SubProcesso> ObterSubProcessos(string processo)
        {
            return _subProcessoRepositorio.GetList(processo).ToList();
        }

        public SubProcesso ObterSubProcessoId(string subprocesso)
        {
            return _subProcessoRepositorio.GetById(subprocesso).ToList().Single();
        }

        public List<SubProcesso> ObterSubProcessos()
        {
            return _subProcessoRepositorio.GetAll().ToList();
        }
    }
}
