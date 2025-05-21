using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;

public class HtmlWordService
{
    public byte[] ConvertHtmlToWord(string html)
    {
        using (var mem = new MemoryStream())
        {
            using (var doc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
            {
                // Agregar parte principal
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = new Body();

                // Limpiar HTML básico (sin HtmlToOpenXml)
                var textoPlano = Regex.Replace(html, "<.*?>", "");
                var lineas = textoPlano.Split('\n');

                foreach (var linea in lineas)
                {
                    var parrafo = new Paragraph(new Run(new Text(linea.Trim())));
                    body.Append(parrafo);
                }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            return mem.ToArray();
        }
    }
}
