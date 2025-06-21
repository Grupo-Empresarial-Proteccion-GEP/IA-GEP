public interface ILanguageModelService
{
    Task<string> SendPromptAsync(string prompt);
}
