using Futebox.Models;
using Futebox.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Processo SalvarProcessoClassificacao(Models.Enums.EnumCampeonato campeonato);
        Processo SalvarProcessoRodada(Models.Enums.EnumCampeonato campeonato, int rodada);

        Processo AgendarProcesso(string id, DateTime hora);

        Task GerarImagem(Processo processo, SubProcesso sub);
        Task GerarAudio(Processo processo, SubProcesso sub);
        Task GerarVideo(Processo processo, SubProcesso sub);
        Task PublicarVideo(Processo processo, SubProcesso sub);
        Task AbrirPasta(Processo processo);

        void AtualizarStatus(ref Processo processo, ref SubProcesso subProcesso, StatusProcesso status);

        Processo AtualizarRoteiro(Processo processo);

        bool Delete(string id);
    }
}
