namespace Futebox.Models
{
    public class Dimensao
    {
        public int Largura { get; set; }
        public int Altura { get; set; }

        public Dimensao(int w, int h)
        {
            Largura = w;
            Altura = h;
        }
    }
}
