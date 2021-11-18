using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public Processo ObterProcesso(string id)
        {
            return _processoService.ObterProcesso(id);
        }

        [HttpGet("{id}/erro")]
        public Processo AtualizarProcessoErro(string id, string mensagem)
        {
            var erro = string.IsNullOrEmpty(mensagem) ? "erro" : mensagem;
            return _processoService.AtualizarProcessoErro(id, erro);
        }

        [HttpGet("{id}/sucesso")]
        public Processo AtualizarProcessoSucesso(string id, string arquivo)
        {
            return _processoService.AtualizarProcessoSucesso(id, arquivo);
        }

        [HttpPost("{id}/deletar")]
        public bool DeletarProcesso(string id)
        {
            return _processoService.Delete(id);
        }

        [HttpPost("partida/{id}")]
        public Processo AddProcessoPartida(string id)
        {
            return _processoService.SalvarProcessoPartida(int.Parse(id));
        }

        [HttpPost("classificacao/{id}")]
        public Processo AddProcessoClassificacao(string id)
        {
            return _processoService.SalvarProcessoClassificacao((Campeonatos)int.Parse(id));
        }

        [HttpPost("rodada/{id}/{rodada}")]
        public Processo AddProcessoRodada(string id, string rodada)
        {
            return _processoService.SalvarProcessoRodada((Campeonatos)int.Parse(id), int.Parse(rodada));
        }

        [HttpGet("executar/{id}")]
        public Processo ExecutarProcesso(string id)
        {
            return _processoService.GerarVideoProcesso(id);
        }

        [HttpGet("arquivos/{id}")]
        public bool ArquivosProcesso(string id)
        {
            return _processoService.ArquivosProcesso(id);
        }

        [HttpGet("notificar/{id}")]
        public bool NotificarProcesso(string id)
        {
            var p = _processoService.ObterProcesso(id);
            _notifyService.Notify(p.notificacao);
            return true;
        }

        [HttpGet("agendar/{id}")]
        public bool AgendarNotificacaoProcesso(string id, string porta, int hora, int minuto)
        {
            var p = _processoService.AtualizarProcessoAgendamento(
                id,
                porta,
                DateTime.Today.AddHours(hora).AddMinutes(minuto)
                );
            _agendamentoService.AgendarExecucao(p.id, p.agendamento ?? DateTime.Now);
            return true;
        }

        [HttpGet("publicar/{id}")]
        public bool PublicarVideoProcesso(string id)
        {
            _processoService.PublicarVideo(id);
            return true;
        }

    }
}
