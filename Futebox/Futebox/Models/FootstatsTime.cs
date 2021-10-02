namespace Futebox.Models
{
    public class FootstatsTime
    {
        public string urlLogo { get; set; }
        public string nome { get; set; }
        public int idTecnico { get; set; }
        public string tecnico { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public bool hasScout { get; set; }
        public Sde sde { get; set; }
        public bool isTimeGrande { get; set; }
        public bool selecao { get; set; }
        public string torcedorNoSingular { get; set; }
        public string torcedorNoPlural { get; set; }
        public bool timeFantasia { get; set; }
        public string grupo { get; set; }
        public string sigla { get; set; }
        public int id { get; set; }

        public class Sde
        {
            public int equipe_id { get; set; }
        }
    }
}
