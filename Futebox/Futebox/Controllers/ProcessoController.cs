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

        [HttpGet("{id}/{sub}/obter")]
        public async Task<Dictionary<string, object>> ObterProcesso(string id, string sub)
        {
            return new Dictionary<string, object>()
            {
                { "processo", _processoService.ObterProcesso(id) },
                { "subprocesso", _processoService.ObterSubProcessoId(sub) }
            };
        }

        [HttpGet("{id}/{sub}/sub")]
        public async Task<SubProcesso> ObterSubProcesso(string id, string sub)
        {
            var subs = _processoService.ObterSubProcessos(id).Find(_ => _.id == sub);
            return subs;
        }

        [HttpPost("{id}/deletar")]
        public async Task<bool> DeletarProcesso(string id)
        {
            return _processoService.Delete(id);
        }

        [HttpPost("partida/{id}")]
        public async Task<Processo> AddProcessoPartida(string id)
        {
            return _processoService.SalvarProcessoPartida(int.Parse(id));
        }

        [HttpPost("classificacao/{id}")]
        public async Task<Processo> AddProcessoClassificacao(string id)
        {
            return _processoService.SalvarProcessoClassificacao((Campeonatos)int.Parse(id));
        }

        [HttpPost("rodada/{id}/{rodada}")]
        public async Task<Processo> AddProcessoRodada(string id, string rodada)
        {
            return _processoService.SalvarProcessoRodada((Campeonatos)int.Parse(id), int.Parse(rodada));
        }

        [HttpGet("notificar/{id}")]
        public async Task<bool> NotificarProcesso(string id)
        {
            var p = _processoService.ObterProcesso(id);
            _notifyService.Notify(p.notificacao);
            return true;
        }

        [HttpGet("agendar/{id}")]
        public async Task<bool> AgendarNotificacaoProcesso(string id, int dia, int mes, int ano, int hr, int min)
        {
            var dt = new DateTime(ano, mes, dia, hr, min, 0);
            var p = _processoService.AgendarProcesso(id, dt);
            _agendamentoService.AgendarExecucao(p.id, p.agendamento ?? DateTime.Now);
            return true;
        }

        [HttpGet("robo/imagem/{id}/{sub}")]
        public async Task<bool> GerarImagem(string id, string sub)
        {
            var processo = _processoService.ObterProcesso(id);
            var subprocesso = _processoService.ObterSubProcessoId(sub);
            _processoService.GerarImagem(ref processo, ref subprocesso);
            return true;
        }

        [HttpGet("robo/audio/{id}/{sub}")]
        public async Task<bool> GerarAudio(string id, string sub)
        {
            var processo = _processoService.ObterProcesso(id);
            var subprocesso = _processoService.ObterSubProcessoId(sub);
            _processoService.GerarAudio(ref processo, ref subprocesso);
            return true;
        }

        [HttpGet("robo/video/{id}/{sub}")]
        public async Task<bool> GerarVideo(string id, string sub)
        {
            var processo = _processoService.ObterProcesso(id);
            var subprocesso = _processoService.ObterSubProcessoId(sub);
            _processoService.GerarVideo(ref processo, ref subprocesso);
            return true;
        }

        [HttpGet("robo/publicar/{id}/{sub}")]
        public async Task<bool> PublicarVideo(string id, string sub)
        {
            var processo = _processoService.ObterProcesso(id);
            var subprocesso = _processoService.ObterSubProcessoId(sub);
            _processoService.PublicarVideo(ref processo, ref subprocesso);
            return true;
        }

        [HttpGet("robo/pasta/{id}")]
        public async Task<bool> PastaVideo(string id)
        {
            var processo = _processoService.ObterProcesso(id);
            _processoService.AbrirPasta(processo);
            return true;
        }
    }
}
