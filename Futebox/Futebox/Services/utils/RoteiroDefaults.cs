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

        public static string TraduzirHoras(DateTime dt)
        {
            var hora = int.Parse(dt.ToString("HH"));
            var min = int.Parse(dt.ToString("mm"));
            if (~~min > 0) return $"{hora} e { min}";
            return $"{hora} horas";
        }

        public static string TraduzirDiaDoMesAno(DateTime dt)
        {
            var dia = dt.ToString("dd");
            var diaInt = int.Parse(dia);
            if (diaInt == 1) dia = "primeiro";
            return $"{diaInt} de { dt.ToString("MMMM") } de { dt.ToString("yyyy") }";
        }

        public static string TraduzirDiaDoMes(DateTime dt)
        {
            var dia = dt.ToString("dd");
            var diaInt = int.Parse(dia);
            if (diaInt == 1) dia = "primeiro";
            return $"{diaInt} de { dt.ToString("MMMM") }";
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
    }
}
