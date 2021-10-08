using Futebox.Models;
using System.Collections.Generic;

namespace Futebox.Services.Interfaces
{
    public interface IProcessoService
    {
        List<Processo> ObterProcessos();
        Processo ObterProcesso(string id);
        
        Processo SalvarProcessoPartida(int idPartida);
        Processo SalvarProcessoClassificacao(Enums.Campeonatos campeonato);
        Processo SalvarProcessoRodada(Enums.Campeonatos campeonato, int rodada);

        bool ExecutarProcesso(string processo);
        Processo AtualizarProcesso(string id, bool processado, string erro = "");

        bool Delete(string id);
    }
}
