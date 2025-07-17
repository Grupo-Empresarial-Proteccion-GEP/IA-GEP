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
                var parts = model.Imagen.Split(',');
                var metadata = parts[0];
                var base64Data = parts[1];

                var extension = "png";
                if (metadata.Contains("image/jpeg")) extension = "jpg";
                else if (metadata.Contains("image/gif")) extension = "gif";
                else if (metadata.Contains("image/bmp")) extension = "bmp";
                else if (metadata.Contains("image/svg+xml")) extension = "svg";

                var imageBytes = Convert.FromBase64String(base64Data);

                var image = builder.LinkedResources.Add($"imagenPlantilla.{extension}", imageBytes);
                image.ContentId = MimeUtils.GenerateMessageId();

                // Esta es la clave: insertar la imagen en el cuerpo del correo
                builder.HtmlBody = $"{model.Mensaje}<br><br><img src=\"cid:{image.ContentId}\" style='max-width:100%;'>";

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al procesar imagen base64 embebida: {ex.Message}");
                builder.HtmlBody = model.Mensaje;
            }
        }
        else
        {
            builder.HtmlBody = model.Mensaje;
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
