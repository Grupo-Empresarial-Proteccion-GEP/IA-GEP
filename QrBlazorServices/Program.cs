using System.ServiceModel;
using AccesoDatos;
using Microsoft.AspNetCore.Http.Features;
using QrBlazorServices.Components;
using Radzen;

namespace QrBlazorServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ============================================
            // 🧩 Servicios
            // ============================================
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddRadzenComponents();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();

            // Servicios SOAP
            builder.Services.AddScoped<AccesoDatosSoapClient>(sp =>
                new AccesoDatosSoapClient(
                    AccesoDatosSoapClient.EndpointConfiguration.AccesoDatosSoap12));

            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<GeminiService>();
            builder.Services.AddScoped<otraIA>();

            // Selección de servicio IA por defecto
            builder.Services.AddScoped<ILanguageModelService>(provider =>
                provider.GetRequiredService<GeminiService>());

            builder.Services.AddScoped<ChatService>();

            // Blazor Server y SignalR configurado para archivos grandes
            builder.Services.AddServerSideBlazor()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                })
                .AddHubOptions(options =>
                {
                    options.MaximumReceiveMessageSize = 50 * 1024 * 1024; // 50 MB
                });

            // Aumentar límite de formularios multipart (archivos)
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
            });

            // Configuración de Kestrel para aceptar archivos grandes
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
            });

            // ============================================
            // 🚀 Construcción de la App
            // ============================================

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
