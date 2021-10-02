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


namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : ControllerBase
    {
        readonly IPartidasService _partidasService;
        readonly IFutebotService _futebotService;

        public PartidasController(IPartidasService partidasService, IFutebotService futebotService)
        {
            _partidasService = partidasService;
            _futebotService = futebotService;
        }

        [HttpGet("discurso")]
        public string GetDiscurso(string partida)
        {
            return _partidasService.ObterRoteiroDaPartida(int.Parse(partida));
        }

        [HttpGet("video")]
        public bool GetVideo(string partida)
        {
            _futebotService.GerarVideoPartida(int.Parse(partida));
            return true;
        }
    }
}
