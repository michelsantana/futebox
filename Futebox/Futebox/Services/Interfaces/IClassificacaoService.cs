using Futebox.Models;
using System;
using System.Collections.Generic;
using Futebox.Models.Enums;

namespace Futebox.Services.Interfaces
{
    public interface IClassificacaoService
    {
        IEnumerable<ClassificacaoVM> ObterClassificacaoPorCampeonato(Models.Enums.EnumCampeonato campeonato, bool usarCache = true);
        IEnumerable<ClassificacaoVM> ObterClassificacaoPorCampeonatoFase(Models.Enums.EnumCampeonato campeonato, string fase, bool usarCache = true);

        string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs);
        Tuple<string, string> ObterAtributosDoVideo(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs);
    }
}
