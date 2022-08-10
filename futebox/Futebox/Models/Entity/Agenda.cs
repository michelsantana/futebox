using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Futebox.Models
{
    public class Agenda : BaseEntity
    {
        public String processoId { get; set; }
        public String descricao { get; set; }
        public DateTime? agendamento { get; set; }
        public Status status { get; set; }

        public enum Status
        {
            criado = 0,
            agendado = 10,
            cancelado = 20,
            executando = 30,
            falha = 40,
            erro = 50,
            concluido = 60
        }
    }
}
