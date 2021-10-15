using Futebox.DB;
using System;

namespace Futebox.Models
{
    public class Processo : BaseEntity
    {
        public Tipo tipo { get; set; }
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

        public int status { get; set; }
        public string statusMensagem { get; set; }

        public bool processado { get; set; }
        public String json { get; set; }

        public enum Tipo
        {
            partida,
            classificacao,
            rodada,
        }

        public enum TipoLink
        {
            print,
            image
        }

        public enum Status
        {
            Pendente = 8,
            Executando = 16,
            Sucesso = 32,
            Erro = 64
        }
    }
}
