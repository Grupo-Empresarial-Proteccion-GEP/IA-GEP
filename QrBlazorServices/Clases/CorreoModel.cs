using Microsoft.AspNetCore.Components.Forms;

namespace QrBlazorServices.Clases
{
    public class CorreoModel
    {
        public string Remitente { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Destinatario { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public List<IBrowserFile> Adjuntos { get; set; } = new();
        public string? Imagen { get; set; }
        public string? Nombre { get; set; }
    }

}
