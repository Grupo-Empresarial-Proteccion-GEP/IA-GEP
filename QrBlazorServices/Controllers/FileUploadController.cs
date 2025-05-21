using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace QrBlazorServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile UploadedFile)
        {
            if (UploadedFile == null) return BadRequest("Archivo no válido");

            var ext = Path.GetExtension(UploadedFile.FileName).ToLower();
            using var ms = new MemoryStream();
            await UploadedFile.CopyToAsync(ms);

            string extractedText;
            if (ext == ".pdf") extractedText = ExtractTextFromPdf(ms);
            else if (ext == ".docx") extractedText = ExtractTextFromDocx(ms);
            else if (ext == ".txt")
            {
                ms.Position = 0;
                using var reader = new StreamReader(ms);
                extractedText = await reader.ReadToEndAsync();
            }
            else return BadRequest("Formato no soportado");

            return Ok(extractedText);
        }

        private string ExtractTextFromPdf(Stream stream)
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

        private string ExtractTextFromDocx(Stream stream)
        {
            stream.Position = 0;
            using var wordDoc = WordprocessingDocument.Open(stream, false);
            var body = wordDoc.MainDocumentPart.Document.Body;
            return body?.InnerText ?? "";
        }
    }
}
