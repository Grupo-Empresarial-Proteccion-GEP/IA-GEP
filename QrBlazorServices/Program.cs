using AccesoDatos;
using QrBlazorServices.Components;
using Radzen;
using System.ServiceModel;

namespace QrBlazorServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddRadzenComponents();

            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<AccesoDatos.AccesoDatosSoapClient>(sp =>
    new AccesoDatos.AccesoDatosSoapClient(
        AccesoDatos.AccesoDatosSoapClient.EndpointConfiguration.AccesoDatosSoap12));

            builder.Services.AddScoped<GeminiService>();     // OpenAI
            builder.Services.AddScoped<otraIA>();             // Google Gemini

            // Indicar cuál IA usar por defecto (por ejemplo GeminiService)
            builder.Services.AddScoped<ILanguageModelService>(provider =>
                provider.GetRequiredService<GeminiService>());

            // ChatService ahora podrá inyectar ILanguageModelService correctamente
            builder.Services.AddScoped<ChatService>();


            builder.Services.AddServerSideBlazor()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
