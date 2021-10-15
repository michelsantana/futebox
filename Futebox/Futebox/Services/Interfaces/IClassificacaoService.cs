using Futebox.Models;
using System;
using System.Collections.Generic;
using Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IClassificacaoService
    {
        IEnumerable<ClassificacaoVM> ObterClassificacaoPorCampeonato(Campeonatos campeonato, bool clearCache = false);
        string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, Campeonatos campeonato);
        Tuple<string, string> ObterAtributosDoVideo(IEnumerable<ClassificacaoVM> classificacao, Campeonatos campeonato);
    }
}
