using Futebox.Models.Enums;

namespace Futebox.Models
{
    public class ProcessoClassificacaoArgs
    {
        public Enums.EnumCampeonato campeonato { get; set; }
        public ProcessoClassificacaoArgs(Enums.EnumCampeonato c)
        {
            campeonato = c;
        }
        public ProcessoClassificacaoArgs(int c)
        {
            campeonato = (Enums.EnumCampeonato)c;
        }
        public ProcessoClassificacaoArgs()
        {

        }
    }

    public class ProcessoRodadaArgs
    {
        public Enums.EnumCampeonato campeonato { get; set; }
        public int rodada { get; set; }
        public ProcessoRodadaArgs(Enums.EnumCampeonato c, int r)
        {
            campeonato = c;
            rodada = r;
        }
        public ProcessoRodadaArgs(int c, int r)
        {
            campeonato = (Enums.EnumCampeonato)c;
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
