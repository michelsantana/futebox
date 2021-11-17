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

namespace Futebox.Services
{
    public class ProcessoService : IProcessoService
    {
        IProcessoRepositorio _processoRepositorio;
        IPartidasService _partidasService;
        IClassificacaoService _classificacaoService;
        IRodadaService _rodadaService;

        public ProcessoService(IProcessoRepositorio processoRepositorio, IPartidasService partidasService, IClassificacaoService classificacaoService, IRodadaService rodadaService)
        {
            _processoRepositorio = processoRepositorio;
            _partidasService = partidasService;
            _classificacaoService = classificacaoService;
            _rodadaService = rodadaService;
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
            var partidas = _partidasService.ObterPartidasHoje(true);
            return SalvarProcessoPartida(partidas.First(_ => _.idExterno == idPartida));
        }
        private Processo SalvarProcessoPartida(PartidaVM partida)
        {
            var roteiro = _partidasService.ObterRoteiroDaPartida(partida);
            var atributos = _partidasService.ObterAtributosDoVideo(partida);
            var processo = new Processo()
            {
                idExterno = partida.idExterno.ToString(),
                tipo = TipoProcesso.partida,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=print",
                linkThumb = $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=thumb",
                tipoLink = TipoLink.print,
                imgAltura = 1920,
                imgLargura = 1080,
                args = JsonConvert.SerializeObject(new ProcessoPartidaArgs(partida.idExterno)),
                roteiro = roteiro,
                status = StatusProcesso.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = $"{atributos.Item2}\n{ObterDescricaoDefault()}",

                agendado = false,
                agendamento = partida.dataPartida.AddHours(2).AddMinutes(10),
                notificacao =
                $"processo: partida" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            return processo;
        }

        public Processo SalvarProcessoClassificacao(Campeonatos campeonato)
        {
            var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(campeonato, true);
            return SalvarProcessoClassificacao(classificacao, campeonato);
        }
        private Processo SalvarProcessoClassificacao(IEnumerable<ClassificacaoVM> classificacao, Campeonatos campeonato)
        {
            var roteiro = _classificacaoService.ObterRoteiroDaClassificacao(classificacao, campeonato);
            var atributos = _classificacaoService.ObterAtributosDoVideo(classificacao, campeonato);
            var processo = new Processo()
            {
                idExterno = DateTime.Now.ToString("yyyyMMddhhmmss"),
                tipo = TipoProcesso.classificacao,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}classificacao?campeonato={(int)campeonato}&viewMode=print",
                //linkThumb = $"{Settings.ApplicationHttpBaseUrl}classificacao?foco={(int)campeonato}&viewMode=thumb",
                tipoLink = TipoLink.print,
                imgAltura = 1080,
                imgLargura = 1920,
                args = JsonConvert.SerializeObject(new ProcessoClassificacaoArgs(campeonato)),
                roteiro = roteiro,
                status = StatusProcesso.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = $"{atributos.Item2}\n{ObterDescricaoDefault()}",

                agendado = false,
                agendamento = DateTime.Today.AddHours(23).AddMinutes(30),
                notificacao =
                $"processo: classificacao" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            return processo;
        }

        public Processo SalvarProcessoRodada(Campeonatos campeonato, int rodada)
        {
            var partidas = _rodadaService.ObterPartidasDaRodada(campeonato, rodada);
            return SalvarProcessoRodada(partidas, campeonato, rodada);

        }
        public Processo SalvarProcessoRodada(IEnumerable<PartidaVM> partidas, Campeonatos campeonato, int rodada)
        {
            var roteiro = _rodadaService.ObterRoteiroDaRodada(partidas, campeonato, rodada);
            var atributos = _rodadaService.ObterAtributosDoVideo(partidas, campeonato, rodada);
            var processo = new Processo()
            {
                idExterno = DateTime.Now.ToString("yyyyMMddhhmmss"),
                tipo = TipoProcesso.rodada,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}rodadas?campeonato={(int)campeonato}&rodada={rodada}&viewMode=print",
                //linkThumb = $"{Settings.ApplicationHttpBaseUrl}rodadas?foco={(int)campeonato}&rodada={rodada}&viewMode=thumb",
                tipoLink = TipoLink.print,
                imgAltura = 1080,
                imgLargura = 1920,
                args = JsonConvert.SerializeObject(new ProcessoRodadaArgs(campeonato, rodada)),
                roteiro = roteiro,
                status = StatusProcesso.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = $"{atributos.Item2}\n{ObterDescricaoDefault()}",

                agendado = false,
                agendamento = DateTime.Now.AddMinutes(30),
                notificacao =
                $"processo: rodada" +
                $"\nnome: {atributos.Item1}",
            };
            _processoRepositorio.Insert(ref processo);
            return processo;
        }

        public Processo ExecutarProcesso(string processo)
        {
            string commandArgs = $"command=executar";
            string idArgs = $"id={processo}";
            string datasourceArgs = $"datasource={Settings.ApplicationHttpBaseUrl}api/processo/{processo}";

            AtualizarRoteiro(processo);

            ExecutarCMD(commandArgs, idArgs, datasourceArgs);
            return _processoRepositorio.GetById(processo);
        }

        public bool ArquivosProcesso(string processo)
        {
            return ExecutarCMD($"command=pasta", $"id={processo}");
        }

        public Processo PublicarVideo(string processo)
        {
            string commandArgs = $"command=publicar";
            string idArgs = $"id={processo}";
            string datasourceArgs = $"datasource={Settings.ApplicationHttpBaseUrl}api/processo/{processo}";
            ExecutarCMD(commandArgs, idArgs, datasourceArgs);
            return _processoRepositorio.GetById(processo);
        }

        public Processo AtualizarProcessoAgendamento(string id, string porta, DateTime hora)
        {
            var p = _processoRepositorio.GetById(id);
            p.agendamento = hora;
            p.agendado = true;
            p.portaExecucao = porta;
            _processoRepositorio.OpenTransaction();
            _processoRepositorio.Update(p);
            _processoRepositorio.Commit();
            return p;
        }

        public Processo AtualizarProcessoSucesso(string id, string arquivo)
        {
            var p = _processoRepositorio.GetById(id);
            p.alteracao = DateTime.Now;
            p.processado = true;
            p.status = StatusProcesso.Sucesso;
            p.arquivoVideo = arquivo;

            _processoRepositorio.OpenTransaction();
            _processoRepositorio.Update(p);
            _processoRepositorio.Commit();

            return p;
        }

        public Processo AtualizarProcessoErro(string id, string erro)
        {
            var p = _processoRepositorio.GetById(id);
            p.alteracao = DateTime.Now;
            p.processado = true;
            p.status = StatusProcesso.Erro;
            p.statusMensagem = erro;

            _processoRepositorio.OpenTransaction();
            _processoRepositorio.Update(p);
            _processoRepositorio.Commit();

            return p;
        }

        public Processo AtualizarRoteiro(Processo processo)
        {
            switch (processo.tipo)
            {
                case TipoProcesso.partida:
                    return AtualizarRoteiroPartida(processo);
                case TipoProcesso.classificacao:
                    return AtualizarRoteiroClassificacao(processo);
                case TipoProcesso.rodada:
                    return AtualizarRoteiroRodada(processo);
            }
            return processo;
        }

        public bool Delete(string id)
        {
            _processoRepositorio.Delete(id);
            return true;
        }

        private bool ExecutarCMD(params object[] args)
        {
            string botFolder = $"{Settings.ApplicationsRoot}/Robot";
            string botBatch = @"integration.bat";
            string argsBuild(object[] arg) => string.Join(" ", arg.Select(_ => $"\"{_}\""));

            string strCmdText = $"{botFolder}/{botBatch}";
            ProcessStartInfo processInfo = new ProcessStartInfo(strCmdText, $"{botFolder} {argsBuild(args)}");
            processInfo.UseShellExecute = true;
            Process batchProcess = new Process();
            batchProcess.StartInfo = processInfo;
            batchProcess.Start();
            batchProcess.WaitForExit();
            return true;
        }

        private Processo AtualizarRoteiro(string processo)
        {
            return AtualizarRoteiro(ObterProcesso(processo));
        }

        private Processo AtualizarRoteiroPartida(Processo processo)
        {
            var args = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(processo.args);
            processo.roteiro = _partidasService.ObterRoteiroDaPartida(args.partidaId);
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

        private string ObterDescricaoDefault()
        {
            var arr = new string[] { "BRASILEIRÃO", "BRASILEIRÃO 2021",
            "BRASILEIRAO", "BRASILEIRAO 2021", "#BRASILEIRAO", "#BRASILEIRAO2021",
            "TABELA BRASILEIRÃO", "TABELA BRASILEIRÃO 2021", "TABELA BRASILEIRAO",
            "TABELA BRASILEIRAO 2021", "#TABELABRASILEIRAO", "#TABELABRASILEIRAO2021",
            "TABELA CLASSIFICAÇÃO", "TABELA CLASSIFICAÇÃO 2021", "TABELA CLASSIFICACAO",
            "TABELA CLASSIFICACAO 2021", "#TABELACLASSIFICACAO", "#TABELACLASSIFICACAO2021",
            "CLASSIFICAÇÃO DO BRASILEIRÃO","CLASSIFICAÇÃO DO BRASILEIRÃO 2021", "CLASSIFICACAO DO BRASILEIRAO",
            "CLASSIFICACAO DO BRASILEIRAO 2021","#CLASSIFICACAODOBRASILEIRAO", "#CLASSIFICACAODOBRASILEIRAO2021",
            "CAMPEONATO BRASILEIRO", "CAMPEONATO BRASILEIRO 2021", "CAMPEONATO BRASILEIRO",
            "CAMPEONATO BRASILEIRO 2021","#CAMPEONATOBRASILEIRO", "#CAMPEONATOBRASILEIRO2021" };
            return string.Join("\n", arr);
        }

    }
}
