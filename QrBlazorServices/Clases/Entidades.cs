namespace QrBlazorServices.Clases
{
    public class Entidad
    {

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int TipoEntidadId { get; set; }
        public string? Tipo { get; set; } // ← para el nombre del tipo
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Departamento { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }


    public class Subdivision
    {
        public int Id { get; set; }              // ✅ Necesario para identificar
        public int EntidadId { get; set; }       // ✅ Necesario para filtrar por entidad

        public string NombreSubdiv { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Responsable { get; set; }
    }


    public class Servicio
    {
        public int Id { get; set; }
        public int EntidadId { get; set; }
        public string ServicioBrindado { get; set; }
    }


}
