// <copyright file="RssFeedsHttpRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Data.RssFeedsData
{
    using System.Net;

    public class RssFeedsHttpRequest
    {
        public async Task<string> GetRssData(string uri)
        {
            try
            {
                var response = await this.GetAsync(() => new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri(uri) });
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
            catch (Exception e)
            {
                Console.WriteLine("Get rss data failed: " + e.Message);
                return string.Empty;
            }
        }

        async Task<HttpResponseMessage> GetAsync(Func<HttpRequestMessage> requestGenerator)
        {
            return await this.RequestAsync(() => requestGenerator());
        }

        private async Task<HttpResponseMessage> RequestAsync(Func<HttpRequestMessage> func)
        {
            var response = await this.ProcessRequestAsync(func);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await this.ProcessRequestAsync(func);
            }

            return response;
        }

        private async Task<HttpResponseMessage> ProcessRequestAsync(Func<HttpRequestMessage> func)
        {
            var client = new HttpClient();
            var response = await client.SendAsync(func()).ConfigureAwait(false);
            return response;
        }
    }
}
