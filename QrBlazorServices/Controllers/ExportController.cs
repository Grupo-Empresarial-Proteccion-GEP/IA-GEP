using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;


[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    [HttpPost]
    public IActionResult ExportHtml([FromBody] string html)
    {
        var service = new HtmlWordService();
        var fileBytes = service.ConvertHtmlToWord(html);

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "documento.docx"
        );
    }
}

