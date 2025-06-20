using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class GeminiService // Sigue llamándose así para no romper el resto de tu código
{

    private string claveAPI = string.Empty;
    private string mensajeClave = string.Empty;

    private readonly AccesoDatos.AccesoDatosSoapClient _servicioUsuario =
        new AccesoDatos.AccesoDatosSoapClient(AccesoDatos.AccesoDatosSoapClient.EndpointConfiguration.AccesoDatosSoap12);

    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
    }

    private async Task ObtenerClaveAPI()
    {
        try
        {
            string campos = "ApiKey";
            string condicion = "1 = 1";
            int limite = 18;

            var resultado = await _servicioUsuario.TraerTablaParametrosAsync("API", campos, condicion, limite);

            if (resultado != null && resultado.Rows.Count > 0)
            {
                claveAPI = resultado.Rows[0]["ApiKey"].ToString().Trim();

                mensajeClave = "Clave API encontrada correctamente.";
            }
            else
            {
                mensajeClave = "No se encontró ninguna clave API en la tabla.";
            }
        }
        catch (Exception ex)
        {
            mensajeClave = $"Error al obtener la clave API: {ex.Message}";
        }
    }

    public async Task<string> SendPromptAsync(string userInput)
    {
        // Obtener la clave desde la BD antes de hacer la solicitud
        await ObtenerClaveAPI();

        if (string.IsNullOrWhiteSpace(claveAPI))
            return $"No se pudo obtener la clave API. {mensajeClave}";

        // Agregar el header de autenticación
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claveAPI);

        var requestBody = new
        {
            model = "gpt-4o", // Cambia por "gpt-3.5-turbo" si es necesario
            messages = new[]
            {
                new { role = "user", content = userInput }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("chat/completions", requestBody);

        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
            var message = responseJson?.choices?.FirstOrDefault()?.message?.content ?? "No hay respuesta.";

            var promptTokens = responseJson?.usage?.prompt_tokens ?? 0;
            var completionTokens = responseJson?.usage?.completion_tokens ?? 0;
            var totalTokens = responseJson?.usage?.total_tokens ?? 0;
            await GuardarConsumoTokens(responseJson);


            return $"{message}\n\n🧾 Tokens usados:\n- Prompt: {promptTokens}\n- Respuesta: {completionTokens}\n- Total: {totalTokens}";

        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
        }
    }
   private async Task GuardarConsumoTokens(OpenAIResponse respuesta)
{
    try
    {
        if (respuesta?.usage == null)
        {
            Console.WriteLine("La respuesta no contiene datos de uso.");
            return;
        }

        DataTable tabla = new DataTable("ConsumoTokens");
        tabla.Columns.Add("Id", typeof(int));
        tabla.Columns.Add("PromptTokens", typeof(int));
        tabla.Columns.Add("CompletionTokens", typeof(int));
        tabla.Columns.Add("TotalTokens", typeof(int));

        DataRow fila = tabla.NewRow();
        fila["Id"] = 0;
        fila["PromptTokens"] = respuesta.usage.prompt_tokens;
        fila["CompletionTokens"] = respuesta.usage.completion_tokens;
        fila["TotalTokens"] = respuesta.usage.total_tokens;

        tabla.Rows.Add(fila);

        await _servicioUsuario.InsertarTablaAsync(tabla, "ConsumoTokens", 18); // Asegúrate de que el ID sea correcto
        Console.WriteLine("✅ Tokens guardados.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al guardar tokens: {ex.Message}");
    }
}





    // Clases para deserializar la respuesta de OpenAI
    public class OpenAIResponse
    {
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}

public class ConsumoTokens
{
    public int Id { get; set; }
    public string Usuario { get; set; }
    public DateTime FechaHora { get; set; }
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
    public decimal CostoUSD { get; set; }
    public string TextoSolicitado { get; set; }
    public string Modelo { get; set; }
}


