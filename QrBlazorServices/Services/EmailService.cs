using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using QrBlazorServices.Clases;

public class EmailService
{
    public async Task EnviarCorreo(CorreoModel model)
    {
        var mensaje = new MimeMessage();
        mensaje.From.Add(MailboxAddress.Parse(model.Remitente));
        mensaje.To.Add(MailboxAddress.Parse(model.Destinatario));
        mensaje.Subject = model.Asunto;

        var builder = new BodyBuilder
        {
            HtmlBody = model.Mensaje
        };

       if (!string.IsNullOrWhiteSpace(model.Imagen) && model.Imagen.StartsWith("data:image"))
{
    try
    {
        // Extraer metadata
        var parts = model.Imagen.Split(',');
        var metadata = parts[0]; // data:image/png;base64
        var base64Data = parts[1];

        // Obtener extensión desde metadata
        var extension = "png"; // por defecto
        if (metadata.Contains("image/jpeg")) extension = "jpg";
        else if (metadata.Contains("image/gif")) extension = "gif";
        else if (metadata.Contains("image/bmp")) extension = "bmp";
        else if (metadata.Contains("image/svg+xml")) extension = "svg";

        // Convertir base64 a bytes
        var imageBytes = Convert.FromBase64String(base64Data);

        // Adjuntar como archivo
        builder.Attachments.Add($"plantilla.{extension}", imageBytes, new ContentType("image", extension));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al procesar la imagen base64: {ex.Message}");
    }
}


        // Adjuntar otros archivos normales
        foreach (var archivo in model.Adjuntos)
        {
            var stream = archivo.OpenReadStream(25 * 1024 * 1024);
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            builder.Attachments.Add(archivo.Name, ms.ToArray(), ContentType.Parse(archivo.ContentType));
        }

        mensaje.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(model.Remitente, model.Contrasena);
        await smtp.SendAsync(mensaje);
        await smtp.DisconnectAsync(true);
    }



}
