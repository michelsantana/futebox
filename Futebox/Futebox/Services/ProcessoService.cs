using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Futebox.Models.Enums;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Futebox.Providers;
using Futebox.Services.utils;

namespace Futebox.Services
{
    public class ProcessoService : IProcessoService
    {
        IProcessoRepositorio _processoRepositorio;
        IPartidasService _partidasService;
        IClassificacaoService _classificacaoService;
        IRodadaService _rodadaService;
        IFutebotService _futebotService;
        IAgendamentoService _agendamentoService;
        IRoteiroProvider _roteiro;

        public ProcessoService(IProcessoRepositorio processoRepositorio, IPartidasService partidasService, IClassificacaoService classificacaoService, IRodadaService rodadaService, IFutebotService futebotService, IAgendamentoService agendamentoService, IRoteiroProvider roteiro)
        {
            _processoRepositorio = processoRepositorio;
            _partidasService = partidasService;
            _classificacaoService = classificacaoService;
            _rodadaService = rodadaService;
            _futebotService = futebotService;
            _agendamentoService = agendamentoService;
            _roteiro = roteiro;
        }

        public async Task<List<Processo>> ObterProcessos()
        {
            return _processoRepositorio.GetAll()?.ToList();
        }

        public async Task<Processo> Obter(string id)
        {
            return _processoRepositorio.GetSingle(_ => _.id == id);
        }

        public async Task<List<Processo>> SalvarProcessoPartida(ProcessoPartidaArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                _agendamentoService.Criar(processo.id);
                result.Add(processo);
            }
            return result;
        }

        public async Task<List<Processo>> SalvarProcessoClassificacao(ProcessoClassificacaoArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                Task.WaitAll(new Task[] { _agendamentoService.Criar(processo.id) });
                result.Add(processo);
            }
            return result;
        }

        public async Task<List<Processo>> SalvarProcessoRodada(ProcessoRodadaArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                _agendamentoService.Criar(processo.id);
                result.Add(processo);
            }
            return result;
        }

        public async Task<List<Processo>> SalvarProcessoJogosDia(ProcessoJogosDiaArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                _agendamentoService.Criar(processo.id);
                result.Add(processo);
            }
            return result;
        }

        public async Task<Processo> AgendarProcesso(string id, DateTime hora)
        {
            Processo p = null;
            await Promise.All(() =>
                {
                    var p = _processoRepositorio.GetById(id);

                    p.agendamento = hora;
                    //p.status = StatusProcesso.Agendado;
                    p.agendado = true;

                    _processoRepositorio.OpenTransaction();
                    _processoRepositorio.Update(p);
                    _processoRepositorio.Commit();
                },
                () => _agendamentoService.Agendar(id, hora)
            );
            return p;
        }

        public async Task<Processo> CancelarAgendamento(string id)
        {
            Processo p = null;
            Task.WaitAll(new Task[]{
                Task.Run(() =>
                {
                    var p = _processoRepositorio.GetById(id);

                    p.agendado = false;

                    _processoRepositorio.OpenTransaction();
                    _processoRepositorio.Update(p);
                    _processoRepositorio.Commit();
                }),
                Task.Run(() =>_agendamentoService.Cancelar(id))
            });
            return p;
        }

        public async Task GerarImagem(Processo processo)
        {
            processo = await AtualizarStatus(processo, StatusProcesso.GerandoImagem);
            var resultado = await _futebotService.GerarImagem(processo);
            await AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                processo = await AtualizarStatus(processo, StatusProcesso.ImagemOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                await AtualizarStatus(processo, StatusProcesso.ImagemErro);
                throw new Exception("GerarImagem"); ;
            }
        }

        public async Task GerarAudio(Processo processo)
        {
            await AtualizarRoteiro(processo);
            processo = await AtualizarStatus(processo, StatusProcesso.GerandoAudio);

            RobotResultApi resultado = await _futebotService.GerarAudio(processo, true, true);

            await AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                processo = await AtualizarStatus(processo, StatusProcesso.AudioOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                await AtualizarStatus(processo, StatusProcesso.AudioErro);
                throw new Exception("GerarAudio"); ;
            }
        }

        public async Task GerarVideo(Processo processo)
        {
            await AtualizarAtributos(processo);
            processo = await AtualizarStatus(processo, StatusProcesso.GerandoVideo);

            var resultado = await _futebotService.GerarVideo(processo);
            await AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                processo = await AtualizarStatus(processo, StatusProcesso.VideoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                await AtualizarStatus(processo, StatusProcesso.VideoErro);
                throw new Exception("GerarVideo"); ;
            }
        }

        public async Task PublicarVideo(Processo processo)
        {
            //AtualizarRoteiro(processo);
            processo = await AtualizarStatus(processo, StatusProcesso.Publicando);
            var resultado = await _futebotService.PublicarVideo(processo);
            await AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                processo = await AtualizarStatus(processo, StatusProcesso.PublicandoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                await AtualizarStatus(processo, StatusProcesso.PublicandoErro);
                throw new Exception("PublicarVideo"); ;
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                await AtualizarStatus(processo, StatusProcesso.Erro);
                throw new Exception("PublicarVideo"); ;
            }
            if (resultado.status == HttpStatusCode.Unauthorized)
            {
                await AtualizarStatus(processo, StatusProcesso.Erro);
                throw new Exception("PublicarVideo"); ;
            }
        }

        public async Task AbrirPasta(Processo processo)
        {
            var resultado = await _futebotService.AbrirPasta(processo);
            if (resultado.status == HttpStatusCode.InternalServerError) throw new Exception("Erro ao gerar o vídeo!");
            if (resultado.status == HttpStatusCode.BadRequest) throw new Exception("Comando inválido");
        }

        public async Task<Processo> AtualizarStatus(Processo processo, StatusProcesso status)
        {
            //var p = _processoRepositorio.GetById(id);
            if (!Directory.Exists(processo.pasta)) Directory.CreateDirectory(processo.pasta);

            processo.alteracao = DateTime.Now;
            processo.status = status;
            return _processoRepositorio.UpdateReturn(processo);
        }

        public async Task AtualizarProcessoLog(Processo processo, string[] lines)
        {
            processo.alteracao = DateTime.Now;
            processo.log = $"{processo.log}\n[UPDATE:{processo.alteracao}]\n{string.Join("\n", lines)}";

            _processoRepositorio.Update(processo);
        }

        private async Task<Processo> AtualizarRoteiro(Processo processo)
        {
            switch (processo.categoria)
            {
                case CategoriaVideo.partida:
                    return AtualizarRoteiroPartida(processo);
                case CategoriaVideo.classificacao:
                    return AtualizarRoteiroClassificacao(processo);
                case CategoriaVideo.rodada:
                    return AtualizarRoteiroRodada(processo);
                case CategoriaVideo.jogosdia:
                    return AtualizarRoteiroJogosDia(processo);
            }
            return processo;
        }

        private async Task<Processo> AtualizarAtributos(Processo processo)
        {
            Tuple<string, string> atributos = null;
            switch (processo.categoria)
            {
                case CategoriaVideo.partida:
                    var partida = _partidasService.ObterPartida(processo.ToArgs<ProcessoPartidaArgs>().partida);
                    atributos = _partidasService.ObterAtributosDoVideo(partida, processo.ToArgs<ProcessoPartidaArgs>());
                    break;
                case CategoriaVideo.classificacao:
                    var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(processo.ToArgs<ProcessoClassificacaoArgs>().campeonato);
                    atributos = _classificacaoService.ObterAtributosDoVideo(classificacao, processo.ToArgs<ProcessoClassificacaoArgs>());
                    break;
                case CategoriaVideo.rodada:
                    atributos = _rodadaService.ObterAtributosDoVideo(processo.ToArgs<ProcessoRodadaArgs>());
                    break;
                case CategoriaVideo.jogosdia:
                    atributos = _partidasService.ObterAtributosDoVideoJogosDia(processo.ToArgs<ProcessoJogosDiaArgs>());
                    break;
            }

            processo.alteracao = DateTime.Now;
            processo.tituloVideo = atributos.Item1;
            processo.descricaoVideo = atributos.Item2;
            _processoRepositorio.Update(processo);
            return processo;
        }

        public async Task<bool> Delete(string id)
        {
            _processoRepositorio.Delete(id);
            return true;
        }

        private Processo AtualizarRoteiroPartida(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(processo.args);
            var partida = _partidasService.ObterPartida(args.partida);
            processo.roteiro = _roteiro.ObterRoteiroDaPartida(partida);
            _processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroClassificacao(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoClassificacaoArgs>(processo.args);
            var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(args.campeonato, true);
            processo.roteiro = _roteiro.ObterRoteiroDaClassificacao(classificacao, args);
            _processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroRodada(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(processo.args);
            var partidas = _rodadaService.ObterPartidasDaRodada(args.campeonato, args.rodada, true);
            processo.roteiro = _roteiro.ObterRoteiroDaRodada(partidas, args);
            _processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroJogosDia(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoJogosDiaArgs>(processo.args);
            var partidas = _partidasService.ObterPartidasHoje(true);
            processo.roteiro = _roteiro.ObterRoteiroDoJogosDoDia(partidas, args);
            _processoRepositorio.Update(processo);
            return processo;
        }
    }
}
