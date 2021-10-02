using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class RoteiroDefaults
    {
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
            return $"{dia} de { dt.ToString("MMMM") } de { dt.ToString("yyyy") }";
        }

        public static string ObterTrechoEmpate(PartidaVM partida)
        {
            return $"Hoje: {partida.timeMandante.ObterNomeWatson} e {partida.timeVisitante.ObterNomeWatson}: " +
                    $"empataram por {partida.golsMandante} a {partida.golsMandante} no estádio, {partida.estadio}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"e iniciou por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela deedáda no laique. Até a próxima.";
        }

        public static string ObterTrechoVencedor(PartidaVM partida, Time vencedor, Time perdedor)
        {
            var golsVencedor = partida.timeMandante.sigla == vencedor.sigla ? partida.golsMandante : partida.golsVisitante;
            var golsPerdedor = partida.timeMandante.sigla == perdedor.sigla ? partida.golsMandante : partida.golsVisitante;

            return $"Hoje: {vencedor.ObterNomeWatson} venceu o {perdedor.ObterNomeWatson}: " +
                    $"por {golsVencedor} a {golsPerdedor}. " +
                    $"O jogo aconteceu no estádio {partida.estadio} por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela deedáda no laique. Até a próxima.";
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
