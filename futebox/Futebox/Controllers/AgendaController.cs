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
    public class AgendaController : ControllerBase
    {
        readonly IAgendamentoService _agendamentoService;

        public AgendaController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
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
