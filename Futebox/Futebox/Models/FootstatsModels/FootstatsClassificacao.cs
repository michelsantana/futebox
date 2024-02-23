namespace Futebox.Models
{
    public class FootstatsClassificacao
    {
        public int posicao { get; set; }
        public int idCampeonato { get; set; }
        public int jogos { get; set; }
        public string campeonato { get; set; }
        public object divisao { get; set; }
        public int pontos { get; set; }
        public int maximoPontosPossivel { get; set; }
        public int golsPro { get; set; }
        public int golsContra { get; set; }
        public int saldoDeGols { get; set; }
        public int aproveitamento { get; set; }
        public int vitorias { get; set; }
        public int empates { get; set; }
        public int cartoesAmarelos { get; set; }
        public int cartoesVermelhos { get; set; }
        public int derrotas { get; set; }
        public int vitoriasMandante { get; set; }
        public int empatesMandante { get; set; }
        public int derrotasMandante { get; set; }
        public int vitoriasVisitante { get; set; }
        public int empatesVisitante { get; set; }
        public int derrotasVisitante { get; set; }
        public int rodada { get; set; }
        public string nomeDaTaca { get; set; }
        public string grupo { get; set; }
        public string fase { get; set; }
        public int idEquipe { get; set; }
        public string equipe { get; set; }
        public int id { get; set; }
    }
}
