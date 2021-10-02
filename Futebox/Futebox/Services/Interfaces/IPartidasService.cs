using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IPartidasService
    {
        IEnumerable<PartidaVM> ObterPartidasHoje(bool skipCache = false);
        string ObterRoteiroDaPartida(PartidaVM partida);
        string ObterRoteiroDaPartida(int idPartida);
        Tuple<string, string> ObterAtributosDoVideo(PartidaVM partida);
    }
}
