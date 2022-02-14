using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class FootstatsPartida
    {
        public int idCampeonato { get; set; }
        //public bool hasScout { get; set; }
        //public Sde sde { get; set; }
        public string estadio { get; set; }
        public int rodada { get; set; }
        public Placar placar { get; set; }
        //public bool hasNarracao { get; set; }
        //public bool temCartaoAmareloTecnicoMandante { get; set; }
        //public bool temCartaoAmareloTecnicoVisitante { get; set; }
        //public bool temCartaoVermelhoTecnicoMandante { get; set; }
        //public bool temCartaoVermelhoTecnicoVisitante { get; set; }
        //public int quantidadeCartaoAmareloComissaoMandante { get; set; }
        //public int quantidadeCartaoVermelhoComissaoMandante { get; set; }
        //public int quantidadeCartaoAmareloComissaoVisitante { get; set; }
        //public int quantidadeCartaoVermelhoComissaoVisitante { get; set; }
        public int idEquipeMandante { get; set; }
        public int? idTecnicoMandante { get; set; }
        public string tecnicoMandante { get; set; }
        public int idEquipeVisitante { get; set; }
        public int? idTecnicoVisitante { get; set; }
        public string tecnicoVisitante { get; set; }
        public int idEstadio { get; set; }
        public string periodoJogo { get; set; }
        public bool partidaEncerrada { get; set; }
        public bool tempoReal { get; set; }
        public bool dataIndefinida { get; set; }
        public bool horaIndefinida { get; set; }
        public DataDaPartida dataDaPartida { get; set; }
        public DateTime? dataDaPartidaIso { get; set; }
        //public int idArbitro { get; set; }
        //public string arbitro { get; set; }
        //public object publico { get; set; }
        //public object playoffChave { get; set; }
        //public string nomeDaTaca { get; set; }
        //public object grupo { get; set; }
        //public int idPeriodoJogo { get; set; }
        //public string fase { get; set; }
        //public object renda { get; set; }
        public int id { get; set; }

        public class Sde
        {
            //public int jogo_id { get; set; }
            //public object sede_id { get; set; }
            //public object arbitro_auxiliar_1_id { get; set; }
            //public object arbitro_auxiliar_2_id { get; set; }
            //public int equipe_mandante_id { get; set; }
            //public int equipe_visitante_id { get; set; }
            //public object tecnico_mandante_id { get; set; }
            //public object arbitro_principal_id { get; set; }
            //public int tecnico_visitante_id { get; set; }
        }
        public class Placar
        {
            public string id { get; set; }
            public string golsMandante { get; set; }
            public string golsVisitante { get; set; }
            //public string penaltisMandante { get; set; }
            //public string penaltisVisitante { get; set; }
            //public string empate { get; set; }
            //public string vitoriaMandante { get; set; }
            //public string derrotaVisitante { get; set; }
            //public string derrotaMandante { get; set; }
            //public string vitoriaVisitante { get; set; }
            //public object golsMandanteWo { get; set; }
            //public object golsVisitanteWo { get; set; }
            //public object decisaoPenaltisCertoMandante { get; set; }
            //public object decisaoPenaltisCertoVisitante { get; set; }
            //public object decisaoPenaltisErradoMandante { get; set; }
            //public object decisaoPenaltisErradoVisitante { get; set; }
        }
        public class Chronology
        {
            public string calendarType { get; set; }
            public string id { get; set; }
        }
        public class DataDaPartida
        {
            public int nano { get; set; }
            public int dayOfYear { get; set; }
            public string dayOfWeek { get; set; }
            public string month { get; set; }
            public int dayOfMonth { get; set; }
            public int year { get; set; }
            public int monthValue { get; set; }
            public int hour { get; set; }
            public int minute { get; set; }
            public int second { get; set; }
            public Chronology chronology { get; set; }
        }
    }
}
