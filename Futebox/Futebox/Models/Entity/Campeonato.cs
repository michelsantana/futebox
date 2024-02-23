using Futebox.DB;

namespace Futebox.Models
{
    public class Campeonato : BaseEntity
    {
        public string origem_ext_id { get; set; }
        public bool? ativo { get; set; }
        public string pais { get; set; }
        public string nome { get; set; }
        public string urlLogo { get; set; }
        public string sdeSlug { get; set; }
        public string temporada { get; set; }
        public string categoria { get; set; }
        public bool? temClassificacao { get; set; }
        public bool? temClassificacaoPorGrupo { get; set; }
        public string tipoDeColeta { get; set; }
        public string faseAtual { get; set; }
        public string quantidadeDeEquipes { get; set; }
        public int? rodadaAtual { get; set; }
        public int? quantidadeDeRodadas { get; set; }
        public string nomeDaTaca { get; set; }
        public string apelido { get; set; }
    }
}
