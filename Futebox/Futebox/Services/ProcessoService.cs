using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Futebox.Models.Enums;

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
                tipo = Processo.Tipo.partida,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=print",
                linkThumb = $"{Settings.ApplicationHttpBaseUrl}partidas?partidaId={partida.idExterno}&viewMode=thumb",
                tipoLink = Processo.TipoLink.print,
                imgAltura = 1920,
                imgLargura = 1080,
                json = JsonConvert.SerializeObject(partida),
                roteiro = roteiro,
                status = (int)Processo.Status.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = atributos.Item2
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
                tipo = Processo.Tipo.classificacao,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}classificacao?campeonato={(int)campeonato}&viewMode=print",
                //linkThumb = $"{Settings.ApplicationHttpBaseUrl}classificacao?foco={(int)campeonato}&viewMode=thumb",
                tipoLink = Processo.TipoLink.print,
                imgAltura = 1080,
                imgLargura = 1920,
                json = JsonConvert.SerializeObject(classificacao),
                roteiro = roteiro,
                status = (int)Processo.Status.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = atributos.Item2
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
                tipo = Processo.Tipo.rodada,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}rodadas?campeonato={(int)campeonato}&rodada={rodada}&viewMode=print",
                //linkThumb = $"{Settings.ApplicationHttpBaseUrl}rodadas?foco={(int)campeonato}&rodada={rodada}&viewMode=thumb",
                tipoLink = Processo.TipoLink.print,
                imgAltura = 1080,
                imgLargura = 1920,
                json = JsonConvert.SerializeObject(partidas),
                roteiro = roteiro,
                status = (int)Processo.Status.Pendente,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = atributos.Item2
            };
            _processoRepositorio.Insert(ref processo);
            return processo;
        }

        public Processo ExecutarProcesso(string processo)
        {
            string botFolder = $"{Settings.ApplicationsRoot}/Robot";
            string botBatch = @"integration.bat";
            string args(params object[] arg) => string.Join(" ", arg.Select(_ => $"\"{_}\""));

            string idArgs = $"id={processo}";
            string datasourceArgs = $"datasource={Settings.ApplicationHttpBaseUrl}api/processo/{processo}";

            string strCmdText = $"{botFolder}/{botBatch}";
            ProcessStartInfo processInfo = new ProcessStartInfo(strCmdText, args(botFolder, "processo", idArgs, datasourceArgs));
            processInfo.UseShellExecute = true;
            Process batchProcess = new Process();
            batchProcess.StartInfo = processInfo;
            batchProcess.Start();
            batchProcess.WaitForExit();
            return _processoRepositorio.GetById(processo);
        }

        public Processo AtualizarProcesso(string id, bool processado, string erro = "")
        {
            var p = _processoRepositorio.GetById(id);
            p.alteracao = DateTime.Now;
            p.processado = processado;
            p.status = (int)Processo.Status.Sucesso;

            if (!string.IsNullOrEmpty(erro))
            {
                p.nome = $"{p.nome}";
                p.status = (int)Processo.Status.Erro;
                p.statusMensagem = erro;
            }
            _processoRepositorio.OpenTransaction();
            _processoRepositorio.Update(p);
            _processoRepositorio.Commit();

            return p;
        }

        public bool Delete(string id)
        {
            _processoRepositorio.Delete(id);
            return true;
        }

        public bool ArquivosProcesso(string processo)
        {
            string botFolder = $"{Settings.ApplicationsRoot}/Robot";
            string botBatch = @"integration.bat";
            string args(params object[] arg) => string.Join(" ", arg.Select(_ => $"\"{_}\""));

            string idArgs = $"id={processo}";

            string strCmdText = $"{botFolder}/{botBatch}";
            ProcessStartInfo processInfo = new ProcessStartInfo(strCmdText, args(botFolder, "pasta", idArgs));
            processInfo.UseShellExecute = true;
            Process batchProcess = new Process();
            batchProcess.StartInfo = processInfo;
            batchProcess.Start();
            batchProcess.WaitForExit();
            return true;
        }
    }
}
