window.descargarCarnet = (elementId, fileName) => {
    const element = document.getElementById(elementId);
    if (!element) {
        alert("No se encontró el carnet para descargar.");
        return;
    }

    html2canvas(element, {
        backgroundColor: "#000000",
        scale: 1,
        width: 326,
        height: 205
    }).then(canvas => {
        const link = document.createElement("a");
        link.download = fileName + ".png";
        link.href = canvas.toDataURL("image/png");
        link.click();
    }).catch(error => {
        alert("Error al generar imagen: " + error);
        console.error(error);
    });
};

window.CarnetOPT = (elementId, fileName) => {
    const element = document.getElementById(elementId);
    if (!element) {
        alert("No se encontró el carnet para descargar.");
        return;
    }

    html2canvas(element, {
        backgroundColor: "#000000",
        scale: 1,
        width: 326,
        height: 205
    }).then(canvas => {
        const link = document.createElement("a");
        link.download = fileName + ".png";
        link.href = canvas.toDataURL("image/png");
        link.click();
    }).catch(error => {
        alert("Error al generar imagen: " + error);
        console.error(error);
    });
};



window.descargarCarnetComoPDF = async function () {
    const element = document.getElementById("carnetContainer");
    if (!element) return;

    const canvas = await html2canvas(element, { scale: 2 });
    const imgData = canvas.toDataURL("image/png");

    const { jsPDF } = window.jspdf;
    const pdf = new jsPDF({
        orientation: "landscape",
        unit: "mm",
        format: [86, 54]
    });

    pdf.addImage(imgData, "PNG", 0, 0, 86, 54);
    pdf.save("Carnet.pdf");
};

