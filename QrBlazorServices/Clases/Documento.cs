namespace QrBlazorServices.Clases
{
    public class Documento
    {
        public string? Nombre { get; set; }
        public string? Referencia { get; set; }
        public int? PlantillaId { get; set; }
        public int? NodoCategoriaId { get; set; }
        public string Version { get; set; }
        public string Estado { get; set; }

    }
}
