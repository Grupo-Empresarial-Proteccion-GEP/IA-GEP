public class ChatService
{
    private readonly List<ChatMessage> _chatHistory = new();
    private ILanguageModelService _currentIA;

    public ChatService(ILanguageModelService defaultIA)
    {
        _currentIA = defaultIA;
    }

    public void SetIA(ILanguageModelService newIA)
    {
        _currentIA = newIA;
    }

    public IReadOnlyList<ChatMessage> ChatHistory => _chatHistory;

    public async Task<string> SendMessageAsync(string userMessage)
    {
        _chatHistory.Add(new ChatMessage { Role = "user", Content = userMessage });

        var fullPrompt = string.Join("\n", _chatHistory.Select(msg =>
            msg.Role == "user" ? $"Usuario: {msg.Content}" : $"Asistente: {msg.Content}"
        ));

        var respuesta = await _currentIA.SendPromptAsync(fullPrompt);

        _chatHistory.Add(new ChatMessage { Role = "assistant", Content = respuesta });

        return respuesta;
    }

    public void ClearHistory()
    {
        _chatHistory.Clear();
    }
}