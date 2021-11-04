using Futebox.Models.Enums;

namespace Futebox.Models
{
    public class ProcessoClassificacaoArgs
    {
        public Campeonatos campeonato { get; set; }
        public ProcessoClassificacaoArgs(Campeonatos c)
        {
            campeonato = c;
        }
        public ProcessoClassificacaoArgs(int c)
        {
            campeonato = (Campeonatos)c;
        }
        public ProcessoClassificacaoArgs()
        {

        }
    }

    public class ProcessoRodadaArgs
    {
        public Campeonatos campeonato { get; set; }
        public int rodada { get; set; }
        public ProcessoRodadaArgs(Campeonatos c, int r)
        {
            campeonato = c;
            rodada = r;
        }
        public ProcessoRodadaArgs(int c, int r)
        {
            campeonato = (Campeonatos)c;
            rodada = r;
        }
        public ProcessoRodadaArgs()
        {

        }
    }

    public class ProcessoPartidaArgs
    {
        public int partidaId { get; set; }
        public ProcessoPartidaArgs(string pid)
        {
            this.partidaId = int.Parse(pid);

        }
        public ProcessoPartidaArgs(int pid)
        {
            this.partidaId = pid;
        }
        public ProcessoPartidaArgs()
        {

        }
    }
}
