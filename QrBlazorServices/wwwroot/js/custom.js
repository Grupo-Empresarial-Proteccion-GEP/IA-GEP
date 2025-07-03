// wwwroot/js/custom.js

export function descargarDocDesdeTextoPlano(contenido, nombreArchivo = "documento.doc") {
    const htmlHeader = `<html xmlns:o='urn:schemas-microsoft-com:office:office'
        xmlns:w='urn:schemas-microsoft-com:office:word'
        xmlns='http://www.w3.org/TR/REC-html40'>
        <head><meta charset='utf-8'><title>Documento</title></head><body>`;
    const htmlFooter = "</body></html>";
    const htmlCompleto = htmlHeader + contenido + htmlFooter;

    const blob = new Blob([htmlCompleto], {
        type: 'application/msword;charset=utf-8'
    });

    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = nombreArchivo;
    document.body.appendChild(link);

    setTimeout(() => {
        link.click();
        document.body.removeChild(link);
    }, 0);
}

export function obtenerHtmlEditable(element) {
    return element.innerHTML;
}
