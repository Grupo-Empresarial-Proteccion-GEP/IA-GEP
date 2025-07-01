import jsPDF from 'jspdf';

export function descargarDocYpdfDesdeTextoPlano(contenido) {
    // Descargar Word
    const blobWord = new Blob([contenido], { type: 'application/msword' });
    const linkWord = document.createElement("a");
    linkWord.href = URL.createObjectURL(blobWord);
    linkWord.download = "documento.doc";
    document.body.appendChild(linkWord);
    linkWord.click();
    document.body.removeChild(linkWord);

    // Descargar PDF
    const doc = new jsPDF();

    // Opcional: divide el contenido en líneas si es muy largo
    const lineas = doc.splitTextToSize(contenido, 180);
    doc.text(lineas, 10, 10);

    const pdfBlob = doc.output('blob');
    const linkPDF = document.createElement("a");
    linkPDF.href = URL.createObjectURL(pdfBlob);
    linkPDF.download = "documento.pdf";
    document.body.appendChild(linkPDF);
    linkPDF.click();
    document.body.removeChild(linkPDF);
}
