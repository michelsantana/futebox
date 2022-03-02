using Futebox.Models.Enums;
using System;
using System.Collections.Generic;

namespace Futebox.Models
{
    public interface IProcessoArgs
    {

    }

    public class ProcessoClassificacaoArgs : IProcessoArgs
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

    public class ProcessoRodadaArgs : IProcessoArgs
    {
        public EnumCampeonato campeonato { get; set; }
        public int rodada { get; set; }
        public int[] partidas { get; set; }
        public RedeSocialFinalidade[] social { get; set; }

        public int linhas { get; set; }
        public int colunas { get; set; }

    }

    public class ProcessoPartidaArgs : IProcessoArgs
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
