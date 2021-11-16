using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;

namespace Futebox.Models
{
    public class Processo : BaseEntity
    {
        public TipoProcesso tipo { get; set; }
        public String idExterno { get; set; }
        public String nome { get; set; }

        public String link { get; set; }
        public String linkThumb { get; set; }
        public TipoLink tipoLink { get; set; }

        public int imgLargura { get; set; }
        public int imgAltura { get; set; }

        public String roteiro { get; set; }
        public String attrTitulo { get; set; }
        public String attrDescricao { get; set; }

        public StatusProcesso status { get; set; }
        public string statusMensagem { get; set; }

        public bool processado { get; set; }
        
        public String args { get; set; }

        public string notificacao { get; set; }
        public DateTime? agendamento { get; set; }
        public bool agendado { get; set; }

        public string arquivoVideo { get; set; }
        public string portaExecucao { get; set; }


        //private object objectArgs;
        //public T ObterArgumentosPartida<T>() where T : class
        //{
        //    if (objectArgs == null)
        //    {
        //        if (typeof(T) == typeof(ProcessoPartidaArgs))
        //            objectArgs = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(args);
        //        if (typeof(T) == typeof(ProcessoClassificacaoArgs))
        //            objectArgs = JsonConvert.DeserializeObject<ProcessoClassificacaoArgs>(args);
        //        if (typeof(T) == typeof(ProcessoRodadaArgs))
        //            objectArgs = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(args);
        //    }
        //    return (T)objectArgs;
        //}
    }
}
