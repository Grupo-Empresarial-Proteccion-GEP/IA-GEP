namespace QrBlazorServices.Clases
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? NodoId { get; set; } // si existe relación explícita

    }
}