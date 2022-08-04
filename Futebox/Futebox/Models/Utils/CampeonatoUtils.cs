using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models.Enums;

namespace Futebox.Models
{
    public class CampeonatoUtils
    {
        public struct CampeonatoConfig
        {
            public string[] fases { get; set; }
            public bool classificacaoPorGrupo { get; set; }
        }

        public static Dictionary<Enums.EnumCampeonato, CampeonatoConfig> Config = new Dictionary<Enums.EnumCampeonato, CampeonatoConfig>()
        {
            { Enums.EnumCampeonato.BrasileiraoSerieA2021, new CampeonatoConfig() },
            { Enums.EnumCampeonato.BrasileiraoSerieB2021, new CampeonatoConfig() },
            { Enums.EnumCampeonato.BrasileiraoSerieA2022, new CampeonatoConfig() },
            { Enums.EnumCampeonato.BrasileiraoSerieB2022, new CampeonatoConfig() },
            { Enums.EnumCampeonato.Libertadores2021, new CampeonatoConfig() },
            { Enums.EnumCampeonato.Paulistao2022, new CampeonatoConfig() { classificacaoPorGrupo = true, fases = new string[] { "Primeira Fase", "Quartas de Final", "Semifinal", "Final" } } }
        };

        public static string ObterNomeDoCampeonato(Enums.EnumCampeonato campeonato) =>
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA2021 ? "Brasileirão Série A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA2022 ? "Brasileirão Série A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB2021 ? "Brasileirão Série B" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB2022 ? "Brasileirão Série B" :
            campeonato == Enums.EnumCampeonato.Libertadores2021 ? "Libertadores" :
            campeonato == Enums.EnumCampeonato.Paulistao2022 ? "Paulistão" :
            ""
            ;

        public static string ObterSerieDoCampeonato(Enums.EnumCampeonato campeonato) =>
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA2021 ? "A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA2022 ? "A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB2021 ? "B" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB2022 ? "B" :
            campeonato == Enums.EnumCampeonato.Libertadores2021 ? "" :
            campeonato == Enums.EnumCampeonato.Paulistao2022 ? "" :
            "";

        public static Enums.EnumCampeonato[] ObterCampeonatosAtivos()
        {
            return new Enums.EnumCampeonato[] {
                //Campeonatos.BrasileiraoSerieA,
                //Campeonatos.BrasileiraoSerieB,
                //Campeonatos.Libertadores2021,
                //Enums.EnumCampeonato.Paulistao2022,
                Enums.EnumCampeonato.BrasileiraoSerieA2022,
                Enums.EnumCampeonato.BrasileiraoSerieB2022,
            };
        }
    }
}
