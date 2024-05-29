using Microsoft.Identity.Abstractions;

namespace Demos.Entra.Reauth.Razor.Services
{
    public class MessagesClient
    {
        private HttpClient HttpClient { get; }
        public MessagesClient(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("messages");
        }

        public async Task<string> GetSimpleMessage()
        {
            var response = await HttpClient.GetStringAsync("/messages");
            return response;
        }

        public async Task<string> GetReAuthMessage()
        {
            var response = await HttpClient.GetStringAsync("/messages/reauth");
            return response;
        }
    }
}
