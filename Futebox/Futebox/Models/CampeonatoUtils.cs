using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Futebox.Models.Enums;

namespace Futebox.Models
{
    public class CampeonatoUtils
    {
        public static string ObterNomeDoCampeonato(Campeonatos campeonato) =>
            campeonato == Campeonatos.BrasileiraoSerieA ? "Brasileirão Série A" :
            campeonato == Campeonatos.BrasileiraoSerieB ? "Brasileirão Série B" :
            campeonato == Campeonatos.Libertadores2021 ? "Libertadores" :
            ""
            ;

        public static string ObterSerieDoCampeonato(Campeonatos campeonato) =>
            campeonato == Campeonatos.BrasileiraoSerieA ? "A" :
            campeonato == Campeonatos.BrasileiraoSerieB ? "B" :
            campeonato == Campeonatos.Libertadores2021 ? "" :
            "";

        public static Campeonatos[] ObterCampeonatosAtivos()
        {
            return new Campeonatos[] {
                Campeonatos.BrasileiraoSerieA,
                Campeonatos.BrasileiraoSerieB,
                Campeonatos.Libertadores2021,
            };
        }
    }
}
