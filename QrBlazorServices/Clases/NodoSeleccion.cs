namespace QrBlazorServices.Clases
{
    public class NodoSeleccion
    {
        public int? NodoSeleccionadoId { get; set; }
        public List<NodoCategoria> Hijos { get; set; } = new();
    }
}
