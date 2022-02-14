﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models.Enums;

namespace Futebox.Models
{
    public class CampeonatoUtils
    {
        public static Campeonato GetPaulistao()
        {
            return new Campeonato()
            {
                ativo = true,
                pais = "Brasil",
                nome = "Paulista 2022",
                urlLogo = "https://frontendapiapp.blob.core.windows.net/images/campeonatos/paulista.png",
                sdeSlug = "campeonato-paulista-2022",
                temporada = "2022",
                categoria = "REGIONAL",
                temClassificacao = true,
                temClassificacaoPorGrupo = true,
                tipoDeColeta = "ESTATISTICA_TOTAL",
                faseAtual = "Primeira Fase",
                quantidadeDeEquipes = "16",
                rodadaAtual = 1,
                quantidadeDeRodadas = 12,
                nomeDaTaca = "Paulistão 2022",
                apelido = "Paulistão 2022",
                origem_ext_id = "789"
            };
        }

        public static Dictionary<Enums.EnumCampeonato, Campeonato> Config = new Dictionary<Enums.EnumCampeonato, Campeonato>()
        {
            { Enums.EnumCampeonato.BrasileiraoSerieA, null },
            { Enums.EnumCampeonato.BrasileiraoSerieB, null },
            { Enums.EnumCampeonato.Libertadores2021, null },
            { Enums.EnumCampeonato.Paulistao2022, GetPaulistao() }
        };

        public static string ObterNomeDoCampeonato(Enums.EnumCampeonato campeonato) =>
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA ? "Brasileirão Série A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB ? "Brasileirão Série B" :
            campeonato == Enums.EnumCampeonato.Libertadores2021 ? "Libertadores" :
            campeonato == Enums.EnumCampeonato.Paulistao2022 ? "Paulistão" :
            ""
            ;

        public static string ObterSerieDoCampeonato(Enums.EnumCampeonato campeonato) =>
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieA ? "A" :
            campeonato == Enums.EnumCampeonato.BrasileiraoSerieB ? "B" :
            campeonato == Enums.EnumCampeonato.Libertadores2021 ? "" :
            campeonato == Enums.EnumCampeonato.Paulistao2022 ? "" :
            "";

        public static Enums.EnumCampeonato[] ObterCampeonatosAtivos()
        {
            return new Enums.EnumCampeonato[] {
                //Campeonatos.BrasileiraoSerieA,
                //Campeonatos.BrasileiraoSerieB,
                //Campeonatos.Libertadores2021,
                Enums.EnumCampeonato.Paulistao2022
            };
        }
    }
}
