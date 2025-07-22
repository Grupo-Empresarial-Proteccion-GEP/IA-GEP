namespace QrBlazorServices.Clases
{
    public class NodoCategoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int? PadreId { get; set; }
        public string? NombreIndentado { get; set; } // ✅ Usado solo para mostrar en DropDown
        public bool EsCategoria { get; set; }
        public bool Expanded { get; set; } = false;
        public List<NodoCategoria> Hijos { get; set; } = new List<NodoCategoria>();
        public bool EsNodoBuscado { get; set; } = false; // 🌟 Para resaltar
        public string NombreOriginal { get; set; }

    }
}
