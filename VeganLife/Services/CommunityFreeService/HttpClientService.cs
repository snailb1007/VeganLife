// <copyright file="HttpClientService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Net.Http.Headers;

namespace VeganLife.Services.CommunityFreeService
{
    internal class HttpClientService
    {
        public static HttpClient Instance { get => lazyClient.Value; }

        private HttpClientService()
        {
        }

        private static readonly Lazy<HttpClient> lazyClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient();

            // Configure client, e.g., set base address, default headers, etc.
            client.BaseAddress = new Uri("https://api.nal.usda.gov/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Other configurations
            return client;
        });
    }
}
