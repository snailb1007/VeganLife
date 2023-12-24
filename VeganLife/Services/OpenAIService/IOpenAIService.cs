namespace VeganLife.Services.OpenAIService
{
    public interface IOpenAIService
    {
        Task<string> AskQuestionAsync(Guid guid, string question);
    }
}
