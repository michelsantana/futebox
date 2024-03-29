﻿using Futebox.DB;
using Futebox.Services;
using Newtonsoft.Json;
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
        public string logoEspecifico { get; set; }

        public string ObterNomeWatson() => string.IsNullOrEmpty(this.nomeAdaptadoWatson) ? this.nome : this.nomeAdaptadoWatson;
        public string ObterLogoLocal()
        {
            if (selecao)
                return string.IsNullOrEmpty(this.logoEspecifico) ? $"./img/escudos/selecoes/-{sigla}.png" : logoEspecifico;
            else 
                return string.IsNullOrEmpty(this.logoEspecifico) ? $"./img/escudos/{pais.RemoverEspacos().RemoverAcentos().ToLower()}/-{sigla}.png" : logoEspecifico;
        }

        public Time()
        {

        }

        public static Time Indefinido()
        {
            return JsonConvert.DeserializeObject<Time>(
    "{ " +
            "\"sigla\": \"º \", " +
            "\"pais\": \"Indefinido\"," +
            "\"nome\": \"1º do grupo A\"," +
            "\"isTimeGrande\": false," +
            "\"selecao\": false," +
            "\"torcedorNoSingular\": null," +
            "\"torcedorNoPlural\": null," +
            "\"timeFantasia\": true," +
            "\"estadio\": \"A Definir\"," +
            "\"idTecnico\": 617," +
            "\"urlLogo\": null," +
            "\"cidade\": \"Indefinido\"," +
            "\"estado\": \"In\"," +
            "\"id\": 1510" +
        "}");
        }
    }
}
