using System.Net.Http.Json;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "AIzaSyDarNXl-QbTg8qWV4CTE5t8EPa0u7EwAdw";

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendPromptAsync(string userInput)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = userInput
                        }
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
}

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
