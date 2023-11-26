namespace VeganLife.Services.OpenAIService
{
    public interface IOpenAIService
    {
        Task<string> AskQuestionAsync(string question);
    }
}
