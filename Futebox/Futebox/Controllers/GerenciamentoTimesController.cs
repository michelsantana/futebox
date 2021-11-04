using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Futebox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GerenciamentoTimesController : ControllerBase
    {

        IGerenciamentoTimesService _gerenciamentoTimesService;
        ICacheHandler _cacheHandler;

        public GerenciamentoTimesController(IGerenciamentoTimesService gerenciamentoTimesService)
        {
            _gerenciamentoTimesService = gerenciamentoTimesService;
        }

        // POST api/<TimeController>
        [HttpPost("{id}")]
        public Time Post(int id)
        {
            var times = _gerenciamentoTimesService.ObterTimesServico();
            var timeParaSalvar = times.First(_ => _.id == id);

            if (timeParaSalvar == null) return null; 

            var timeSalvo = _gerenciamentoTimesService.SalvarTime(timeParaSalvar);
            return timeSalvo;
        }

        // PUT api/<TimeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimeController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _gerenciamentoTimesService.RemoverTimeDB(id);
        }
    }
}
