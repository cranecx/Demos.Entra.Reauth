
using Microsoft.Identity.Abstractions;

namespace Demos.Entra.Reauth.Razor.MessageHandlers
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private IAuthorizationHeaderProvider AuthorizationHeaderProvider { get; }
        public AuthorizationMessageHandler(IAuthorizationHeaderProvider authorizationHeaderProvider)
        {
            AuthorizationHeaderProvider = authorizationHeaderProvider;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await AuthorizationHeaderProvider
                .CreateAuthorizationHeaderForUserAsync(["api://33563f3c-4bfc-4783-82e1-2832406e7acb/messages.retrieve.simple",
                    "api://33563f3c-4bfc-4783-82e1-2832406e7acb/messages.retrieve.reauth"]);

            request.Headers.Add("Authorization", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
