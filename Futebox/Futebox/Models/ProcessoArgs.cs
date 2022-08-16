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
        public string fase { get; set; }

        public RedeSocialFinalidade social { get; set; }
        
        public string[] grupos { get; set; }
        public int[] range { get; set; }

        public bool temFases { get; set; }
        public bool classificacaoPorGrupos { get; set; }

        public string viewName { get; set; }

        public int linhas { get; set; }
        public int colunas { get; set; }
        public string titulo { get; set; }
        public DateTime? dataExecucao { get; set; }
        public ProcessoClassificacaoArgs()
        {
            dataExecucao = dataExecucao ?? DateTime.Now;
        }
    }

    public class ProcessoRodadaArgs : IProcessoArgs
    {
        public EnumCampeonato campeonato { get; set; }
        public int rodada { get; set; }
        public int[] partidas { get; set; }
        public RedeSocialFinalidade social { get; set; }

        public string viewName { get; set; }
        public int linhas { get; set; }
        public int colunas { get; set; }
        public string titulo { get; set; }
    }

    public class ProcessoJogosArgs : IProcessoArgs
    {
        public int[] partidas { get; set; }
        public RedeSocialFinalidade social { get; set; }

        public string viewName { get; set; }
        public int linhas { get; set; }
        public int colunas { get; set; }
        public string titulo { get; set; }
    }
    
    public class ProcessoPartidaArgs : IProcessoArgs
    {
        public int partida { get; set; }
        public RedeSocialFinalidade social { get; set; }
        public string titulo { get; set; }
        public string viewName { get; set; }
    }
}
