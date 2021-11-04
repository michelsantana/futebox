using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : ControllerBase
    {
        readonly IPartidasService _partidasService;

        public PartidasController(IPartidasService partidasService)
        {
            _partidasService = partidasService;
        }

        [HttpGet("discurso")]
        public string GetDiscurso(string partida)
        {
            return _partidasService.ObterRoteiroDaPartida(int.Parse(partida));
        }
    }
}
