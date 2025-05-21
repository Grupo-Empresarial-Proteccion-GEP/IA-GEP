export function descargarDocDesdeTextoPlano(contenido) {
    const blob = new Blob([contenido], { type: 'application/msword' });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = "documento.doc";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
