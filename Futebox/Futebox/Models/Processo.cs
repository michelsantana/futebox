using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;

namespace Futebox.Models
{
    public class Processo : BaseEntity
    {

        public String nome { get; set; }
        public CategoriaVideo categoria { get; set; }
        public StatusProcesso status { get; set; }

        public string statusMensagem { get; set; }

        public string notificacao { get; set; }
        public DateTime? agendamento { get; set; }
        public bool agendado { get; set; }
        public string pastaDosArquivos { get; set; }

        public string partidaId { get; set; }
        public string campeonatoId { get; set; }
        public string rodadaId { get; set; }        

    }
}
