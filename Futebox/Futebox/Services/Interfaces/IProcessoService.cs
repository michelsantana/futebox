using Futebox.Models;
using Futebox.Models.Enums;
using System;
using System.Collections.Generic;

namespace Futebox.Services.Interfaces
{
    public interface IProcessoService
    {
        List<Processo> ObterProcessos();
        Processo ObterProcesso(string id);
        
        Processo SalvarProcessoPartida(int idPartida);
        Processo SalvarProcessoClassificacao(Campeonatos campeonato);
        Processo SalvarProcessoRodada(Campeonatos campeonato, int rodada);

        Processo ExecutarProcesso(string processo);
        bool ArquivosProcesso(string id);
        Processo PublicarVideo(string processo);

        Processo AtualizarProcessoAgendamento(string id, string porta, DateTime hora);
        Processo AtualizarProcessoSucesso(string id, string arquivo);
        Processo AtualizarProcessoErro(string id, string erro);
        Processo AtualizarRoteiro(Processo processo);

        bool Delete(string id);
    }
}
