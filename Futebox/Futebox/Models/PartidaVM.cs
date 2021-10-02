using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class PartidaVM
    {
        public DateTime dataPartida { get; set; }

        public int idExterno { get; set; }

        public Time timeMandante { get; set; }
        public Time timeVisitante { get; set; }

        public string golsMandante { get; set; }
        public string golsVisitante { get; set; }

        public string dataHoraDaPartida { get; set; }
        public string estadio { get; set; }
        public string campeonato { get; set; }
    }
}
