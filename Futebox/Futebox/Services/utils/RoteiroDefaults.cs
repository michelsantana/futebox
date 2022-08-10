using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Futebox.Models.Enums;

namespace Futebox.Services
{
    public class RoteiroDefaults
    {

        public class VerbalizarTempo
        {
            public Tempo tempoVerbal { get; set; }

            public VerbalizarTempo(DateTime dataPartida, DateTime? dataExecucao = null)
            {
                dataExecucao = dataExecucao ?? DateTime.Now;
                var exec = Convert.ToInt32(dataExecucao?.ToString("yyyyMM") ?? "0");
                var referenceDay = Convert.ToInt32(dataPartida.ToString("yyyyMMdd") ?? "0");

                if (exec == referenceDay) this.tempoVerbal = Tempo.presente;
                if (exec > referenceDay) this.tempoVerbal = Tempo.passado;
                if (exec < referenceDay) this.tempoVerbal = Tempo.futuro;
            }

            public enum Tempo
            {
                passado,
                presente,
                futuro
            }
        }

        public static string ObterSaudacao()
        {
            return $"Fala torcedôr e torcedôra: Bem vindo ao canal Futibox: ";
        }

        public static string TraduzirHoras(DateTime dt)
        {
            var hora = int.Parse(dt.ToString("HH"));
            var min = int.Parse(dt.ToString("mm"));
            if (~~min > 0) return $"{hora} e { min}";
            return $"{hora} horas";
        }

        public static string TraduzirDiaDoMes(DateTime dt)
        {
            var dia = dt.ToString("dd");
            var diaInt = int.Parse(dia);
            if (diaInt == 1) dia = "primeiro";
            return $"{diaInt} de { dt.ToString("MMMM") } de { dt.ToString("yyyy") }";
        }

        public static string ObterTrechoEmpate(PartidaVM partida)
        {
            return //$"Hoje: " +
                    $"{partida.timeMandante.ObterNomeWatson()} e {partida.timeVisitante.ObterNomeWatson()}, " +
                    $"empataram por {partida.golsMandante} a {partida.golsMandante} no estádio, {partida.estadio}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"e iniciou por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela deedáda no laique.";
        }

        public static string ObterTrechoVencedor(PartidaVM partida, Time vencedor, Time perdedor)
        {
            var golsVencedor = partida.timeMandante.sigla == vencedor.sigla ? partida.golsMandante : partida.golsVisitante;
            var golsPerdedor = partida.timeMandante.sigla == perdedor.sigla ? partida.golsMandante : partida.golsVisitante;

            return $"{vencedor.ObterNomeWatson()} venceu {perdedor.ObterNomeWatson()}: " +
                    $"por {golsVencedor} a {golsPerdedor} no estádio {partida.estadio}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"e iniciou {RoteiroDefaults.ObterHojeAmanhaOntem(partida.dataPartida)} por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela deedáda no laique.";
        }

        public static string ObterHojeAmanhaOntem(DateTime dataPartida, DateTime? dataReferenciaExecucao = null)
        {
            dataReferenciaExecucao = dataReferenciaExecucao ?? DateTime.Now;
            var yearMonthRef = Convert.ToInt32(dataReferenciaExecucao?.ToString("yyyyMM") ?? "0");
            var dayRef = Convert.ToInt32(dataReferenciaExecucao?.ToString("dd") ?? "0");
            var yearMonth = Convert.ToInt32(dataPartida.ToString("yyyyMM") ?? "0");
            var day = Convert.ToInt32(dataPartida.ToString("dd") ?? "0");
            if (yearMonthRef == yearMonth)
            {
                if (dayRef == day) return "hoje";

                if ((dayRef - 2) == day) return "anteontem";
                if ((dayRef - 1) == day) return "ontem";

                if ((dayRef + 1) == day) return "amanhã";
                if ((dayRef + 2) == day) return "depois de amanhã";
            }
            return TraduzirDiaDoMes(dataPartida);
        }

        public static string RemoverAcentos(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

    }
}
