using Futebox.Models;
using System.Collections.Generic;

namespace Futebox.Services.Roteiros
{
    public interface IRoteiro
    {
        string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs);
        string ObterRoteiroDaPartida(PartidaVM partida);
        string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, ProcessoRodadaArgs processoRodadaArgs);

    }
}
