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
using System.IO;

namespace Futebox.Services
{
    public class ProcessoService : IProcessoService
    {
        IProcessoRepositorio _processoRepositorio;
        IPartidasService _partidasService;
        IClassificacaoService _classificacaoService;
        IRodadaService _rodadaService;
        IFutebotService _futebotService;

        public ProcessoService(IProcessoRepositorio processoRepositorio, IPartidasService partidasService, IClassificacaoService classificacaoService, IRodadaService rodadaService, IFutebotService futebotService)
        {
            _processoRepositorio = processoRepositorio;
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

        public List<Processo> SalvarProcessoPartida(ProcessoPartidaArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                result.Add(processo);
            }
            return result;
        }

        public List<Processo> SalvarProcessoClassificacao(ProcessoClassificacaoArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                result.Add(processo);
            }
            return result;
        }

        public List<Processo> SalvarProcessoRodada(ProcessoRodadaArgs[] args)
        {
            var result = new List<Processo>();
            foreach (var arg in args)
            {
                var processo = new Processo(arg);
                _processoRepositorio.Insert(ref processo);
                result.Add(processo);
            }
            return result;
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

            return p;
        }


        public async Task GerarImagem(Processo processo)
        {
            AtualizarStatus(ref processo, StatusProcesso.GerandoImagem);
            var resultado = await _futebotService.GerarImagem(processo);
            AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, StatusProcesso.ImagemOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, StatusProcesso.ImagemErro);
                throw new Exception("GerarImagem"); ;
            }
        }

        public async Task GerarAudio(Processo processo)
        {
            AtualizarRoteiro(processo);
            AtualizarStatus(ref processo, StatusProcesso.GerandoAudio);

            var resultado = await _futebotService.GerarAudio(processo);
            AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, StatusProcesso.AudioOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, StatusProcesso.AudioErro);
                throw new Exception("GerarAudio"); ;
            }
        }

        public async Task GerarVideo(Processo processo)
        {
            AtualizarAtributos(processo);
            AtualizarStatus(ref processo, StatusProcesso.GerandoVideo);

            var resultado = _futebotService.GerarVideo(processo);
            AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, StatusProcesso.VideoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, StatusProcesso.VideoErro);
                throw new Exception("GerarVideo"); ;
            }
        }

        public async Task PublicarVideo(Processo processo)
        {
            //AtualizarRoteiro(processo);
            AtualizarStatus(ref processo, StatusProcesso.Publicando);
            var resultado = await _futebotService.PublicarVideo(processo);
            AtualizarProcessoLog(processo, resultado.stack.ToArray());

            if (resultado.status == HttpStatusCode.OK)
            {
                AtualizarStatus(ref processo, StatusProcesso.PublicandoOK);
            }
            if (resultado.status == HttpStatusCode.InternalServerError)
            {
                AtualizarStatus(ref processo, StatusProcesso.PublicandoErro);
                throw new Exception("PublicarVideo"); ;
            }
            if (resultado.status == HttpStatusCode.BadRequest)
            {
                AtualizarStatus(ref processo, StatusProcesso.Erro);
                throw new Exception("PublicarVideo"); ;
            }
        }

        public async Task AbrirPasta(Processo processo)
        {
            var resultado = _futebotService.AbrirPasta(processo);
            if (resultado.status == HttpStatusCode.InternalServerError) throw new Exception("Erro ao gerar o vídeo!");
            if (resultado.status == HttpStatusCode.BadRequest) throw new Exception("Comando inválido");
        }


        public void AtualizarStatus(ref Processo processo, StatusProcesso status)
        {
            //var p = _processoRepositorio.GetById(id);
            if (!Directory.Exists(processo.pasta)) Directory.CreateDirectory(processo.pasta);

            processo.alteracao = DateTime.Now;
            processo.status = status;
            _processoRepositorio.Update(processo);
        }

        public void AtualizarProcessoLog(Processo processo, string[] lines)
        {
            processo.alteracao = DateTime.Now;
            processo.log = $"{processo.log}\n[UPDATE:{processo.alteracao}]\n{string.Join("\n", lines)}";

            _processoRepositorio.Update(processo);
        }

        private Processo AtualizarRoteiro(Processo processo)
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

        private Processo AtualizarAtributos(Processo processo)
        {
            Tuple<string, string> atributos = null;
            switch (processo.categoria)
            {
                case CategoriaVideo.partida:
                    var partida = _partidasService.ObterPartida(processo.ToArgs<ProcessoPartidaArgs>().partida);
                    atributos = _partidasService.ObterAtributosDoVideo(partida);
                    break;
                case CategoriaVideo.classificacao:
                    var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(processo.ToArgs<ProcessoClassificacaoArgs>().campeonato);
                    atributos = _classificacaoService.ObterAtributosDoVideo(classificacao, processo.ToArgs<ProcessoClassificacaoArgs>().campeonato);
                    break;
                case CategoriaVideo.rodada:
                    var partidas = _rodadaService.ObterPartidasDaRodada(processo.ToArgs<ProcessoRodadaArgs>().campeonato, processo.ToArgs<ProcessoRodadaArgs>().rodada);
                    atributos = _rodadaService.ObterAtributosDoVideo(partidas, processo.ToArgs<ProcessoRodadaArgs>().campeonato, processo.ToArgs<ProcessoRodadaArgs>().rodada);
                    break;
            }

            processo.alteracao = DateTime.Now;
            processo.tituloVideo = atributos.Item1;
            processo.descricaoVideo = atributos.Item2;
            _processoRepositorio.Update(processo);
            return processo;
        }

        public bool Delete(string id)
        {
            _processoRepositorio.Delete(id);
            return true;
        }

        private Processo AtualizarRoteiroPartida(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(processo.args);
            processo.roteiro = _partidasService.ObterRoteiroDaPartida(args.partida);
            _processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroClassificacao(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoClassificacaoArgs>(processo.args);
            var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(args.campeonato, true);
            processo.roteiro = _classificacaoService.ObterRoteiroDaClassificacao(classificacao, args.campeonato);
            _processoRepositorio.Update(processo);
            return processo;
        }

        private Processo AtualizarRoteiroRodada(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(processo.args);
            var partidas = _rodadaService.ObterPartidasDaRodada(args.campeonato, args.rodada, true);
            processo.roteiro = _rodadaService.ObterRoteiroDaRodada(partidas, args.campeonato, args.rodada);
            _processoRepositorio.Update(processo);
            return processo;
        }
    }
}
