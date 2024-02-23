using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class ClassificacaoVM
    {

        public int posicao { get; set; }
        public string grupo { get; set; }
        public string brasao { get; set; }
        public string clube { get; set; }
        public int pontos { get; set; }
        public int partidasJogadas { get; set; }
        public int vitorias { get; set; }
        public int empates { get; set; }
        public int derrotas { get; set; }
        public int golsPro { get; set; }
        public int golsContra { get; set; }
        public int saldoGols { get; set; }
        public string ultimasCinco { get; set; }
        public string corDestaque { get; set; }
        public string fase { get; set; }
        public Time time { get; set; }

    }
}
