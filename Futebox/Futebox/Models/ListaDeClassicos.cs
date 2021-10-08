﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class ListaDeClassicos
    {
        public class ClassicoVM
        {
            public string nome { get; set; }
            public string time1 { get; set; }
            public string sigla1 { get; set; }
            public string time2 { get; set; }
            public string sigla2 { get; set; }
            public string[] atalho { get; set; }
        }

        static List<ClassicoVM> _list;
        public static List<ClassicoVM> ObterLista()
        {
            if (_list == null)
                _list = JsonConvert.DeserializeObject<List<ClassicoVM>>(json);
            return _list;
        }


        private static string json = @"
[
  {
    ""nome"": ""Athletico-PR x Coritiba"",
    ""time1"": ""Athletico-PR"",
    ""sigla1"": ""CAP"",
    ""time2"": ""Coritiba"",
    ""sigla2"": ""CFC"",
    ""atalho"": [""CAP"", ""CFC""]
    },
  {
    ""nome"": ""Atlético Mineiro x Cruzeiro"",
    ""time1"": ""Atlético Mineiro"",
    ""sigla1"": ""CAM"",
    ""time2"": ""Cruzeiro"",
    ""sigla2"": ""CRU"",
    ""atalho"": [""CAM"", ""CRU""]
},
  {
    ""nome"": ""Bahia x Vitória"",
    ""time1"": ""Bahia"",
    ""sigla1"": ""BAH"",
    ""time2"": ""Vitória"",
    ""sigla2"": ""VIT"",
    ""atalho"": [""BAH"", ""VIT""]
  },
  {
    ""nome"": ""Ceará x Fortaleza"",
    ""time1"": ""Ceará"",
    ""sigla1"": ""CEA"",
    ""time2"": ""Fortaleza"",
    ""sigla2"": ""FOR"",
    ""atalho"": [""CEA"", ""FOR""]
  },
  {
    ""nome"": ""Corinthians x Palmeiras"",
    ""time1"": ""Corinthians"",
    ""sigla1"": ""COR"",
    ""time2"": ""Palmeiras"",
    ""sigla2"": ""PAL"",
    ""atalho"": [""COR"", ""PAL""]
  },
  {
    ""nome"": ""CRB x CSA"",
    ""time1"": ""CRB"",
    ""sigla1"": ""CRB"",
    ""time2"": ""CSA"",
    ""sigla2"": ""CSA"",
    ""atalho"": [""CRB"", ""CSA""]
  },
  {
    ""nome"": ""Flamengo x Vasco"",
    ""time1"": ""Flamengo"",
    ""sigla1"": ""FLA"",
    ""time2"": ""Vasco"",
    ""sigla2"": ""VAS"",
    ""atalho"": [""FLA"", ""VAS""]
  },
  {
    ""nome"": ""Goiás x Vila Nova"",
    ""time1"": ""Goiás"",
    ""sigla1"": ""GOI"",
    ""time2"": ""Vila Nova"",
    ""sigla2"": ""VIL"",
    ""atalho"": [""GOI"", ""VIL""]
  },
  {
    ""nome"": ""Grêmio x Internacional"",
    ""time1"": ""Grêmio"",
    ""sigla1"": ""GRE"",
    ""time2"": ""Internacional"",
    ""sigla2"": ""INT"",
    ""atalho"": [""GRE"", ""INT""]
  },
  {
    ""nome"": ""Guarani x Ponte Preta"",
    ""time1"": ""Guarani"",
    ""sigla1"": ""GFC"",
    ""time2"": ""Ponte Preta"",
    ""sigla2"": ""PON"",
    ""atalho"": [""GFC"", ""PON""]
  },
  {
    ""nome"": ""Paysandu x Remo"",
    ""time1"": ""Paysandu"",
    ""sigla1"": ""PAY"",
    ""time2"": ""Remo"",
    ""sigla2"": ""REM"",
    ""atalho"": [""PAY"", ""REM""]
  },
  {
    ""nome"": ""Sampaio Corrêa x Moto Club"",
    ""time1"": ""Sampaio Corrêa"",
    ""sigla1"": ""SAM"",
    ""time2"": ""Moto Club"",
    ""sigla2"": ""MOT"",
    ""atalho"": [""SAM"", ""MOT""]
  },
  {
    ""nome"": ""Santa Cruz x Sport"",
    ""time1"": ""Santa Cruz"",
    ""sigla1"": ""STA"",
    ""time2"": ""Sport"",
    ""sigla2"": ""SPT"",
    ""atalho"": [""STA"", ""SPT""]
  },
  {
    ""nome"": ""Confiança x Sergipe"",
    ""time1"": ""Confiança"",
    ""sigla1"": ""CON"",
    ""time2"": ""Sergipe"",
    ""sigla2"": ""CSS"",
    ""atalho"": [""CON"", ""CSS""]
  },
  {
    ""nome"": ""ABC x América-RN"",
    ""time1"": ""ABC"",
    ""sigla1"": ""ABC"",
    ""time2"": ""América-RN"",
    ""sigla2"": ""ARN"",
    ""atalho"": [""ABC"", ""ARN""]
  },
  {
    ""nome"": ""Avaí x Figueirense"",
    ""time1"": ""Avaí"",
    ""sigla1"": ""AVA"",
    ""time2"": ""Figueirense"",
    ""sigla2"": ""FIG"",
    ""atalho"": [""AVA"", ""FIG""]
  },
  {
    ""nome"": ""Campinense x Treze"",
    ""time1"": ""Campinense"",
    ""sigla1"": ""CAM"",
    ""time2"": ""Treze"",
    ""sigla2"": ""TRZ"",
    ""atalho"": [""CAM"", ""TRZ""]
  }
]";
    }
}
