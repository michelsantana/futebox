using Futebox.DB;
using Futebox.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class Time : BaseEntity
    {
        public String nome { get; set; }
        public String sigla { get; set; }
        public String urlLogo { get; set; }
        public String origemDado { get; set; }
        public String origem_ext_id { get; set; }
        public String origem_ext_equipe_id { get; set; }
        public String nomeAdaptadoWatson { get; set; }
        public String logoBin { get; set; }

        public Boolean selecao { get; set; }
        public String torcedorNoSingular { get; set; }
        public String torcedorNoPlural { get; set; }
        public Boolean timeFantasia { get; set; }
        public String cidade { get; set; }
        public String estado { get; set; }
        public String pais { get; set; }
        public String grupo { get; set; }
        public Boolean isTimeGrande { get; set; }
        public bool hasScout { get; set; }
        public Int32 idTecnico { get; set; }
        public String tecnico { get; set; }

        public string ObterNomeWatson ()=> string.IsNullOrEmpty(this.nomeAdaptadoWatson) ? this.nome : this.nomeAdaptadoWatson;
        public string ObterLogoLocal() => $"./img/escudos/{pais.RemoverEspacos().RemoverAcentos().ToLower()}/-{sigla}.png";

        public Time()
        {

        }
    }
}
