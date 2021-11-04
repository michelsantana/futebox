using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RodadaController : ControllerBase
    {
        readonly IPartidasService _partidaService;

        public RodadaController(IPartidasService partidaService)
        {
            _partidaService = partidaService;
        }

        [HttpGet("ultimarodada/{campeonato}")]
        public IEnumerable<string> RodadaAtual(int campeonato)
        {
            var partidas = _partidaService.ObterPartidasDoCampeonato((Campeonatos)campeonato);
            var hj = Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd"));
            
            var partidasDoMes = partidas
                .Where(_ => 
                        (Convert.ToInt32(_.dataPartida.ToString("yyyyMMdd")) >= hj - 5)
                        && (Convert.ToInt32(_.dataPartida.ToString("yyyyMMdd")) <= hj + 5)
                )
                .GroupBy(_ => new { 
                    dt = _.dataPartida.ToString("yyyyMMdd"), 
                    rodada = _.rodada 
                })
                .Select(g => $"{g.FirstOrDefault().dataPartida.ToString("dd MMMM")},{g.FirstOrDefault().rodada}")
                ;
            return partidasDoMes;
        }
    }
}
