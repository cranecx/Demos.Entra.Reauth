using Demos.Entra.Reauth.Mvc.Services;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using System.Threading;

namespace Demos.Entra.Reauth.Razor.Services
{
    public class MessagesClient
    {
        private HttpClient HttpClient { get; }
        private ITokenAcquisition TokenAcquirer { get; }
        private MessageClientOptions Options { get; }
        public MessagesClient(IHttpClientFactory httpClientFactory, ITokenAcquisition tokenAcquirer, IOptions<MessageClientOptions> options)
        {
            Options = options.Value;
            HttpClient = httpClientFactory.CreateClient();
            HttpClient.BaseAddress = new(Options.BaseAddress!);
            TokenAcquirer = tokenAcquirer;
        }

        public async Task<string> GetSimpleMessage()
        {
            await AuthorizeClient();
            var response = await HttpClient.GetStringAsync("/messages");
            return response;
        }

        public async Task<string> GetReAuthMessage()
        {
            await AuthorizeClient(true);
            var response = await HttpClient.GetStringAsync("/messages/reauth");
            return response;
        }

        private async Task AuthorizeClient(bool refreshToken = false)
        {
            var accessToken = await TokenAcquirer
                .GetAccessTokenForUserAsync(Options.Scopes,
                     tokenAcquisitionOptions: new TokenAcquisitionOptions
                     {
                         ForceRefresh = refreshToken
                     });

            HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken);
        }
    }
}
