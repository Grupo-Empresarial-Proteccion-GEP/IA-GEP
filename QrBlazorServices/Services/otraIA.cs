using System.Data;
using System.Net.Http.Json;
using System.Net.Http.Headers;

public class otraIA : ILanguageModelService
{
    private string claveAPI = string.Empty;
    private string mensajeClave = string.Empty;

    private readonly HttpClient _httpClient;

    // Servicio SOAP para obtener claves desde la base de datos
    private readonly AccesoDatos.AccesoDatosSoapClient _servicioUsuario =
        new AccesoDatos.AccesoDatosSoapClient(
            AccesoDatos.AccesoDatosSoapClient.EndpointConfiguration.AccesoDatosSoap12
        );

    public otraIA(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task ObtenerClaveAPI()
    {
        try
        {
            string campos = "ApiKey";
            string condicion = "Id = 3"; // ← Filtramos por ID de la API de Google
            int limite = 18;

            var resultado = await _servicioUsuario.TraerTablaParametrosAsync("API", campos, condicion, limite);

            if (resultado != null && resultado.Rows.Count > 0)
            {
                claveAPI = resultado.Rows[0]["ApiKey"].ToString().Trim();
                mensajeClave = "Clave API obtenida correctamente.";
            }
            else
            {
                mensajeClave = "No se encontró clave API con Id = 2.";
            }
        }
        catch (Exception ex)
        {
            mensajeClave = $"Error al obtener la clave API: {ex.Message}";
        }
    }

    public async Task<string> SendPromptAsync(string userInput)
    {
        await ObtenerClaveAPI();

        if (string.IsNullOrWhiteSpace(claveAPI))
            return $"No se pudo obtener la clave API. {mensajeClave}";

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={claveAPI}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = userInput }
                    }
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, requestBody);

        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            return responseJson?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "No hay respuesta.";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
        }
    }

    // Clases de respuesta
    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }
}
