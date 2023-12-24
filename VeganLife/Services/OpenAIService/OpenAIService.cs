using ChatGptNet;
using ChatGptNet.Exceptions;
using VeganLife.Helpers;

namespace VeganLife.Services.OpenAIService
{
    public class OpenAIService : IOpenAIService
    {
        public async Task<string> AskQuestionAsync(Guid guid, string question)
        {
            try
            {
                var response = await ServicesHelper.GetService<IChatGptClient>()
                    .AskAsync(guid, message: question);
                if (response?.IsSuccessful ?? false)
                {
                    return response.GetContent() ?? string.Empty;
                }

                return string.Empty;
            }
            catch (ChatGptException chatEX)
            {
#if DEBUG
                await Console.Out.WriteLineAsync("==> " + chatEX.Message);
#endif
                return string.Empty;
            }
        }
    }
}
