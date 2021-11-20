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

        Processo GerarVideoProcesso(string processo);
        bool AbrirPastaProcesso(string id);
        Processo PublicarVideo(string processo);

        Processo AgendarProcesso(string id, DateTime hora);
        Processo AtualizarProcessoVideoCompleto(string id, string arquivo);
        Processo AtualizarProcessoVideoErro(string id);
        Processo AtualizarProcessoPublicado(string id, string link);
        Processo AtualizarProcessoPublicacaoErro(string id);

        Processo AtualizarRoteiro(Processo processo);

        bool Delete(string id);
    }
}
