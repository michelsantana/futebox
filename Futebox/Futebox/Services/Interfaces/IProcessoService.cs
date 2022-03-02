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

        List<Processo> SalvarProcessoPartida(ProcessoPartidaArgs[] args);
        List<Processo> SalvarProcessoClassificacao(ProcessoClassificacaoArgs[] args);
        List<Processo> SalvarProcessoRodada(ProcessoRodadaArgs[] args);


        Processo AgendarProcesso(string id, DateTime hora);

        Task GerarImagem(Processo processo);
        Task GerarAudio(Processo processo);
        Task GerarVideo(Processo processo);
        Task PublicarVideo(Processo processo);
        Task AbrirPasta(Processo processo);

        void AtualizarStatus(ref Processo processo, StatusProcesso status);

        bool Delete(string id);
    }
}
