using Futebox.Models;
using Futebox.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IProcessoService
    {
        Task<List<Processo>> ObterProcessos();
        Task<Processo> Obter(string id);

        Task<List<Processo>> SalvarProcessoPartida(ProcessoPartidaArgs[] args);
        Task<List<Processo>> SalvarProcessoClassificacao(ProcessoClassificacaoArgs[] args);
        Task<List<Processo>> SalvarProcessoRodada(ProcessoRodadaArgs[] args);
        Task<List<Processo>> SalvarProcessoJogosDia(ProcessoJogosDiaArgs[] args);

        Task<Processo> AgendarProcesso(string id, DateTime hora);

        Task GerarImagem(Processo processo);
        Task GerarAudio(Processo processo);
        Task GerarVideo(Processo processo);
        Task PublicarVideo(Processo processo);
        Task AbrirPasta(Processo processo);

        Task<Processo> AtualizarStatus(Processo processo, StatusProcesso status);

        Task<bool> Delete(string id);
        Task<Processo> CancelarAgendamento(string id);
    }
}
