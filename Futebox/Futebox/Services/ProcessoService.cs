using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Futebox.Models.Enums;

namespace Futebox.Services
{
    public class ProcessoService : IProcessoService
    {
        IProcessoRepositorio _processoRepositorio;
        IPartidasService _partidasService;
        IClassificacaoService _classificacaoService;

        public ProcessoService(IProcessoRepositorio processoRepositorio, IPartidasService partidasService, IClassificacaoService classificacaoService)
        {
            _processoRepositorio = processoRepositorio;
            _partidasService = partidasService;
            _classificacaoService = classificacaoService;
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
            var partidas = _partidasService.ObterPartidasHoje();
            return SalvarProcessoPartida(partidas.First(_ => _.idExterno == idPartida));
        }

        public Processo SalvarProcessoPartida(PartidaVM partida)
        {
            var roteiro = _partidasService.ObterRoteiroDaPartida(partida);
            var atributos = _partidasService.ObterAtributosDoVideo(partida);
            var processo = new Processo()
            {
                idExterno = partida.idExterno.ToString(),
                tipo = Processo.Tipo.partida,
                nome = $"{partida.timeMandante.nome} x {partida.timeMandante.nome} - {partida.dataHoraDaPartida}",
                link = $"{Settings.ApplicationHttpBaseUrl}partidas?partida={partida.idExterno}&printMode=1",
                tipoLink = Processo.TipoLink.print,
                json = JsonConvert.SerializeObject(partida),
                roteiro = roteiro,
                status = 1,
                processado = false,
                attrTitulo = atributos.Item1,
                attrDescricao = atributos.Item2
            };
            _processoRepositorio.Insert(ref processo);
            return processo;
        }

        public Processo SalvarProcessoClassificacao(Campeonatos campeonato)
        {
            var classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(campeonato);
            return SalvarProcessoClassificacao(classificacao, campeonato);
        }

        public Processo SalvarProcessoClassificacao(IEnumerable<ClassificacaoVM> classificacao, Campeonatos campeonato)
        {
            var roteiro = _classificacaoService.ObterRoteiroDaClassificacao(classificacao, campeonato);
            var atributos = _classificacaoService.ObterAtributosDoVideo(classificacao, campeonato);
            var processo = new Processo()
            {
                idExterno = DateTime.Now.ToString("yyyyMMddhhmmss"),
                tipo = Processo.Tipo.partida,
                nome = $"{atributos.Item1}",
                link = $"{Settings.ApplicationHttpBaseUrl}classificacao?foco={(int)campeonato}&printMode=1",
                tipoLink = Processo.TipoLink.print,
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

        public bool ExecutarProcesso(string processo)
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
            return true;
        }

        public Processo AtualizarProcesso(string id, bool processado, string erro = "")
        {
            var p = _processoRepositorio.GetById(id);
            p.alteracao = DateTime.Now;
            p.processado = processado;
            p.status = (int)Processo.Status.Sucesso;

            if (!string.IsNullOrEmpty(erro)) {
                p.nome = $"[Erro] - {p.nome}";
                p.status = (int)Processo.Status.Erro;
                p.attrDescricao = erro;
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
    }
}
