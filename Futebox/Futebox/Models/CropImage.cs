namespace Futebox.Models
{
    public class CropImage
    {
        public bool status { get; set; }
        public string[] inputParams { get; set; }
        public string[] output { get; set; }
        public string[] steps { get; set; }
        public string error { get; set; }
    }
}
