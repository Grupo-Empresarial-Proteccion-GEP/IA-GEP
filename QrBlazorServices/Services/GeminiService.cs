using System.Net.Http.Headers;
using System.Net.Http.Json;

public class GeminiService // Sigue llamándose así para no romper el resto de tu código
{
    private readonly HttpClient _httpClient;
    //private readonly string _apiKey = "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"; // OpenAI Key

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
      //  _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendPromptAsync(string userInput)
    {
        var requestBody = new
        {
            model = "GPT-4o", // Cambia por "gpt-3.5-turbo" si es necesario
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

            return $"{message}\n\n🧾 Tokens usados:\n- Prompt: {promptTokens}\n- Respuesta: {completionTokens}\n- Total: {totalTokens}";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
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