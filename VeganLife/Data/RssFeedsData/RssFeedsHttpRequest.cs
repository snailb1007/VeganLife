using System.Net;

namespace VeganLife.Data.RssFeedsData
{
    public class RssFeedsHttpRequest
    {

        public async Task<string> GetRssData(string uri)
        {
            HttpClient client = new HttpClient();

            try
            {
                var response = await GetAsync(() => new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri(uri) });
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

        public async Task<HttpResponseMessage> GetAsync(Func<HttpRequestMessage> requestGenerator)
        {
            return await RequestAsync(() => requestGenerator());
        }

        public async Task<HttpResponseMessage> RequestAsync(Func<HttpRequestMessage> func)
        {
            var response = await ProcessRequestAsync(func);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await ProcessRequestAsync(func);
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
