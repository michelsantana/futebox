using Futebox.Models;
using System.Collections.Generic;

namespace Futebox.Providers
{
    public interface IRoteiroProvider
    {
        string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs);
        string ObterRoteiroDaPartida(PartidaVM partida);
        string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, ProcessoRodadaArgs processoRodadaArgs);
        string ObterRoteiroDoJogosDoDia(IEnumerable<PartidaVM> partidas, ProcessoJogosDiaArgs args);
    }
}
