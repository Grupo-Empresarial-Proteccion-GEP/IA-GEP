public class otroChatService
{
    private readonly List<ChatMessage> _chatHistory = new();
    private readonly GeminiService _geminiService;

    public otroChatService(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public IReadOnlyList<ChatMessage> ChatHistory => _chatHistory;

    public async Task<string> SendMessageAsync(string userMessage)
    {
        _chatHistory.Add(new ChatMessage { Role = "user", Content = userMessage });

        var fullPrompt = string.Join("\n", _chatHistory.Select(msg =>
            msg.Role == "user" ? $"Usuario: {msg.Content}" : $"Asistente: {msg.Content}"
        ));

        var respuesta = await _geminiService.SendPromptAsync(fullPrompt);

        _chatHistory.Add(new ChatMessage { Role = "assistant", Content = respuesta });

        return respuesta;
    }

    public void ClearHistory()
    {
        _chatHistory.Clear();
    }
}