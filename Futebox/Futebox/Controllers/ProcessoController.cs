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

        [HttpPost("jogosdia")]
        public async Task<List<Processo>> AddProcessoJogosDia([FromBody] ProcessoJogosDiaArgs[] args)
        {
            if (args == null || args?.Length == 0) throw new Exception();
            return await _processoService.SalvarProcessoJogosDia(args);
        }

        [HttpPost("{id}/executar")]
        public async Task<RobotResultApi> ExecutarProcesso(string id)
        {
            return await _execucaoProcessoService.Executar(id);
        }

        [HttpGet("{id}/agendar")]
        public async Task<bool> AgendarProcesso(string id, int dia, int mes, int ano, int hr, int min, int sec)
        {
            var dt = new DateTime(ano, mes, dia, hr, min, sec);
            var p = await _processoService.AgendarProcesso(id, dt);
            return true;
        }

        [HttpGet("{id}/agendar-cancelar")]
        public async Task<bool> AgendarCancelar(string id)
        {
            var p = await _processoService.CancelarAgendamento(id);
            return true;
        }

        [HttpGet("{id}/job")]
        public async Task<Agenda> ObterJob(string id)
        {
            return await _agendamentoService.Obter(id);
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

        [HttpGet("pasta")]
        public async Task<bool> Pasta(string q)
        {
            return (await _futebotService.AbrirPasta(q)).IsOk();
        }

        [HttpGet("{id}/log")]
        public async Task<string> Log(string id)
        {
            return (await ObterProcesso(id)).log;
        }

        [HttpPost("{id}/deletar")]
        public async Task<bool> DeletarProcesso(string id)
        {
            return await _processoService.Delete(id);
        }

        [HttpGet("{id}/imagem")]
        public async Task<Processo> ImagemProcesso(string id)
        {
            var processo = await ObterProcesso(id);
            //if(processo.agendado)
            //    return await _processoService.AtualizarStatus(processo, StatusProcesso.Agendado);
            //else
            return await _processoService.AtualizarStatus(processo, StatusProcesso.Criado);
        }

        [HttpGet("{id}/audio")]
        public async Task<Processo> AudioProcesso(string id)
        {
            var processo = await ObterProcesso(id);
            return await _processoService.AtualizarStatus(processo, StatusProcesso.ImagemOK);
        }

        [HttpGet("{id}/video")]
        public async Task<Processo> VideoProcesso(string id)
        {
            var processo = await ObterProcesso(id);
            return await _processoService.AtualizarStatus(processo, StatusProcesso.AudioOK);
        }

        [HttpGet("{id}/publicar")]
        public async Task<Processo> PublicarProcesso(string id)
        {
            var processo = await ObterProcesso(id);
            return await _processoService.AtualizarStatus(processo, StatusProcesso.VideoOK);
        }

        [HttpGet("{id}/complete")]
        public async Task<Processo> CompletarProcessoManual(string id)
        {
            var processo = await ObterProcesso(id);
            return await _processoService.AtualizarStatus(processo, StatusProcesso.PublicandoOK);
        }
    }
}
