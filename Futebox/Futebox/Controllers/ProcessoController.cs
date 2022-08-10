using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        readonly IProcessoService _processoService;
        readonly IExecucaoProcessoService _execucaoProcessoService;
        readonly INotifyService _notifyService;
        readonly IAgendamentoService _agendamentoService;
        readonly IFutebotService _futebotService;

        public ProcessoController(IProcessoService processoService, IExecucaoProcessoService execucaoProcessoService, INotifyService notifyService, IAgendamentoService agendamentoService, IFutebotService futebotService)
        {
            _processoService = processoService;
            _execucaoProcessoService = execucaoProcessoService;
            _agendamentoService = agendamentoService;
            _notifyService = notifyService;
            _futebotService = futebotService;
        }

        [HttpGet("{id}/obter")]
        public async Task<Processo> ObterProcesso(string id)
        {
            return await _processoService.Obter(id);
        }

        [HttpPost("{id}/deletar")]
        public async Task<bool> DeletarProcesso(string id)
        {
            return await _processoService.Delete(id);
        }

        [HttpPost("partida")]
        public async Task<List<Processo>> AddProcessoPartida([FromBody] ProcessoPartidaArgs[] args)
        {
            if (args == null || args?.Length == 0) throw new Exception();
            return await _processoService.SalvarProcessoPartida(args);
        }

        [HttpPost("classificacao")]
        public async Task<List<Processo>> AddProcessoClassificacao([FromBody] ProcessoClassificacaoArgs[] args)
        {
            if (args == null || args?.Length == 0) throw new Exception();
            return await _processoService.SalvarProcessoClassificacao(args);
        }

        [HttpPost("rodada")]
        public async Task<List<Processo>> AddProcessoRodada([FromBody] ProcessoRodadaArgs[] args)
        {
            if (args == null || args?.Length == 0) throw new Exception();
            return await _processoService.SalvarProcessoRodada(args);
        }

        [HttpGet("agendar/{id}")]
        public async Task<bool> AgendarProcesso(string id, int dia, int mes, int ano, int hr, int min, int sec)
        {
            var dt = new DateTime(ano, mes, dia, hr, min, sec);
            var p = await _processoService.AgendarProcesso(id, dt);
            return true;
        }

        [HttpPost("{id}/start")]
        public async Task<RobotResultApi> ExecutarProcesso(string id)
        {
            return await _execucaoProcessoService.Executar(id);
        }

        [HttpGet("jobs")]
        public async Task<Object> Jobs()
        {

            var all = new List<Quartz.IJobDetail>();
            var running = new List<Quartz.IJobExecutionContext>();

            Task.WaitAll(
                Task.Run(async () => all = await _agendamentoService.JobList()),
                Task.Run(async () => running = await _agendamentoService.JobListRunning())
            );


            return new { all = all.Select(_ => _.Key.Name), running = running };
        }

        //[HttpGet("notificar/{id}")]
        //public async Task<bool> NotificarProcesso(string id)
        //{
        //    var p = _processoService.ObterProcesso(id);
        //    _notifyService.Notify(p.notificacao);
        //    return true;
        //}



        //[HttpGet("robo/imagem/{id}/{sub}")]
        //public async Task<bool> GerarImagem(string id, string sub)
        //{
        //    var processo = _processoService.ObterProcesso(id);
        //    var subprocesso = _processoService.ObterSubProcessoId(sub);
        //    _processoService.GerarImagem(processo, subprocesso);
        //    return true;
        //}

        //[HttpGet("robo/audio/{id}/{sub}")]
        //public async Task<bool> GerarAudio(string id, string sub)
        //{
        //    var processo = _processoService.ObterProcesso(id);
        //    var subprocesso = _processoService.ObterSubProcessoId(sub);
        //    _processoService.GerarAudio(processo, subprocesso);
        //    return true;
        //}

        //[HttpGet("robo/video/{id}/{sub}")]
        //public async Task<bool> GerarVideo(string id, string sub)
        //{
        //    var processo = _processoService.ObterProcesso(id);
        //    var subprocesso = _processoService.ObterSubProcessoId(sub);
        //    _processoService.GerarVideo(processo, subprocesso);
        //    return true;
        //}

        //[HttpGet("robo/publicar/{id}/{sub}")]
        //public async Task<bool> PublicarVideo(string id, string sub)
        //{
        //    var processo = _processoService.ObterProcesso(id);
        //    var subprocesso = _processoService.ObterSubProcessoId(sub);
        //    _processoService.PublicarVideo(processo, subprocesso);
        //    return true;
        //}

        //[HttpGet("robo/pasta/{id}")]
        //public async Task<bool> PastaVideo(string id)
        //{
        //    var processo = _processoService.ObterProcesso(id);
        //    _processoService.AbrirPasta(processo);
        //    return true;
        //}
    }
}
