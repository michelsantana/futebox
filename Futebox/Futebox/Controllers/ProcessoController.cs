using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Futebox.Models.Enums;

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        readonly IProcessoService _processoService;
        readonly IFutebotService _futebotService;

        public ProcessoController(IProcessoService processoService, IFutebotService futebotService)
        {
            _processoService = processoService;
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
            return _processoService.AtualizarProcesso(id, false, erro);
        }

        [HttpGet("{id}/sucesso")]
        public Processo AtualizarProcessoSucesso(string id)
        {
            return _processoService.AtualizarProcesso(id, true);
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
        public bool ExecutarProcesso(string id)
        {
            return _processoService.ExecutarProcesso(id);
        }
    }
}
