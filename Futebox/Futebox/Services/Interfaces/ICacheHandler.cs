using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface ICacheHandler
    {
        string ObterPastaCache();
        T ObterConteudo<T>(string cacheName) where T : class;
        bool DefinirConteudo<T>(string cacheName, T dados, int validadeHoras = 8) where T : class;
    }
}
