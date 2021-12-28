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
        List<SubProcesso> ObterSubProcessos();
        List<SubProcesso> ObterSubProcessos(string processoId);
        SubProcesso ObterSubProcessoId(string subprocessoId);

        Processo SalvarProcessoPartida(int idPartida);
        Processo SalvarProcessoClassificacao(Campeonatos campeonato);
        Processo SalvarProcessoRodada(Campeonatos campeonato, int rodada);

        Processo AgendarProcesso(string id, DateTime hora);

        void GerarImagem(ref Processo processo, ref SubProcesso sub);
        void GerarAudio(ref Processo processo, ref SubProcesso sub);
        void GerarVideo(ref Processo processo, ref SubProcesso sub);
        void PublicarVideo(ref Processo processo, ref SubProcesso sub);
        void AbrirPasta(Processo processo);

        void AtualizarStatus(ref Processo processo, ref SubProcesso subProcesso, StatusProcesso status);

        Processo AtualizarRoteiro(Processo processo);

        bool Delete(string id);
    }
}
