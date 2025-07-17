namespace QrBlazorServices.Clases
{
    public class PlantillaCorreo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Tipo { get; set; }
        public string Variables { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
