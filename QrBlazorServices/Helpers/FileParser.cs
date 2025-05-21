using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace QrBlazorServices.Helpers
{
    public static class FileParser
    {
        public static string ExtractTextFromPdf(Stream stream)
        {
            StringBuilder text = new();
            stream.Position = 0;
            using var reader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(reader);
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                text.AppendLine(PdfTextExtractor.GetTextFromPage(page));
            }
            return text.ToString();
        }

        public static string ExtractTextFromDocx(Stream stream)
        {
            stream.Position = 0;
            using var wordDoc = WordprocessingDocument.Open(stream, false);
            var body = wordDoc.MainDocumentPart.Document.Body;
            return body?.InnerText ?? "";
        }
    }
}
