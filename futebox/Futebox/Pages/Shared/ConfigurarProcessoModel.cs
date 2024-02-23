using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Pages
{
    public class ConfigurarProcessoModel
    {
        public Futebox.Models.Enums.RedeSocialFinalidade[] redeSocials { get; set; }
        public string tituloPadrao { get; set; }
        public Mode modo { get; set; }

        public bool mostrarCampoData { get; set; }
        public bool mostrarCampoDataRelativa { get; set; }
        public bool mostrarCampoRange { get; set; }
        public DateTime? dataExecucaoRetroativa { get; set; }

        public enum Mode
        {
            view,
            aspect
        }
    }
}
