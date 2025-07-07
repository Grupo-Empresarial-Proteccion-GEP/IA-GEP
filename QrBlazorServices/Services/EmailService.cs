using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using QrBlazorServices.Controllers;

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
            TextBody = model.Mensaje
        };

        // ✅ Agregar adjuntos
        foreach (var archivo in model.Adjuntos)
        {
            if (archivo != null)
            {
                using var stream = archivo.OpenReadStream(10 * 1024 * 1024); // 10MB máximo
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                builder.Attachments.Add(archivo.Name, ms.ToArray(), ContentType.Parse(archivo.ContentType));
            }
        }

        mensaje.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(model.Remitente, model.Contrasena); // Contraseña de aplicación
        await smtp.SendAsync(mensaje);
        await smtp.DisconnectAsync(true);
    }
}
