namespace QrBlazorServices.Clases
{
    public class DocumentoSeleccion
    {
        public int DocumentoId { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string Nodo { get; set; }

        // Nuevos campos relacionados con la plantilla
        public string? Referencia { get; set; }
        public string? Version { get; set; }
        public string? Estado { get; set; }
        public string? RutaArchivo { get; set; }

        public string NombreCompleto => $"{Nombre}";
    }
}
