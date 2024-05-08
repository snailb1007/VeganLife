// <copyright file="OpenAIService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using ChatGptNet;
using ChatGptNet.Exceptions;
using ChatGptNet.Extensions;
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
                await Console.Out.WriteLineAsync($"==> Failed code: {chatEX.StatusCode}\n{chatEX.Message}");
#endif
                return string.Empty;
            }
        }
    }
}
