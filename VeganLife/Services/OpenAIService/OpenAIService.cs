// <copyright file="OpenAIService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using ChatGptNet;
using ChatGptNet.Exceptions;
using ChatGptNet.Extensions;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Resources.Translations;

namespace VeganLife.Services.OpenAIService
{
    public class OpenAIService : IOpenAIService
    {
        public async Task<string> AskQuestionAsync(Guid guid, string question)
        {
            try
            {
                var response = await FFImageLoading.Helpers.ServiceHelper.GetService<IChatGptClient>()
                    .AskAsync(guid, message: question);
                if (response?.IsSuccessful ?? false)
                {
                    return response.GetContent() ?? string.Empty;
                }

                return string.Empty;
            }
            catch (ChatGptException chatEX)
            {
                chatEX.LogError(description: $"{chatEX.StatusCode}");
                return AppResources.app_common_gptUnableEx;
            }
        }
    }
}
