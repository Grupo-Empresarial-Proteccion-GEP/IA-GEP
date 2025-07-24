namespace QrBlazorServices.Clases
{
    public class DocumentoExtendido
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Referencia { get; set; }
        public int? PlantillaId { get; set; }
        public string? NombrePlantilla { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public string? CategoriaNombre { get; set; }
        public string? NodoFinalNombre { get; set; }
        public string? Categoria { get; set; }
        public string? SubCategoria { get; set; }
        public string? Nodo { get; set; }
        public string RutaBase64 { get; set; }
    }
    }

