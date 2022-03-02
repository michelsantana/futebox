using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        readonly IProcessoService _processoService;
        readonly INotifyService _notifyService;
        readonly IAgendamentoService _agendamentoService;
        readonly IFutebotService _futebotService;

        public ProcessoController(IProcessoService processoService, INotifyService notifyService, IAgendamentoService agendamentoService, IFutebotService futebotService)
        {
            _processoService = processoService;
            _agendamentoService = agendamentoService;
            _notifyService = notifyService;
            _futebotService = futebotService;
        }

        [HttpGet("{id}/obter")]
        public async Task<Processo> ObterProcesso(string id)
        {
            return _processoService.ObterProcesso(id);
        }

        [HttpPost("{id}/deletar")]
        public async Task<bool> DeletarProcesso(string id)
        {
            return _processoService.Delete(id);
        }

        [HttpPost("partida")]
        public async Task<List<Processo>> AddProcessoPartida([FromBody] ProcessoPartidaArgs[] args)
        {
            throw new NotImplementedException();
        }

        [HttpPost("classificacao")]
        public async Task<List<Processo>> AddProcessoClassificacao([FromBody] ProcessoClassificacaoArgs[] args)
        {
            throw new NotImplementedException();
        }

        [HttpPost("rodada")]
        public async Task<List<Processo>> AddProcessoRodada([FromBody] ProcessoRodadaArgs[] args)
        {
            if (args == null || args?.Length == 0) throw new Exception();
            return _processoService.SalvarProcessoRodada(args);
        }

        [HttpGet("agendar/{id}")]
        public async Task<bool> AgendarNotificacaoProcesso(string id, int dia, int mes, int ano, int hr, int min)
        {
            var dt = new DateTime(ano, mes, dia, hr, min, 0);
            var p = _processoService.AgendarProcesso(id, dt);
            _agendamentoService.AgendarExecucao(p.id, p.agendamento ?? DateTime.Now);
            return true;
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
