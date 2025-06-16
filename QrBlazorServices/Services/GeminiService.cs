using System.Net.Http.Headers;
using System.Net.Http.Json;

public class GeminiService // Sigue llamándose así para no romper el resto de tu código
{
    private readonly HttpClient _httpClient;
// private readonly string _apiKey = ""; // OpenAI Key

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendPromptAsync(string userInput)
    {
        var requestBody = new
        {
            model = "gpt-4o", // O usa "gpt-3.5-turbo" si no tienes acceso
            messages = new[]
            {
                new { role = "user", content = userInput }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("chat/completions", requestBody);

        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
            return responseJson?.choices?.FirstOrDefault()?.message?.content ?? "No hay respuesta.";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
        }
    }

    // Solo necesitas esta clase para deserializar la respuesta de OpenAI
    public class OpenAIResponse
    {
        public Choice[] choices { get; set; }
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
}
////
///
