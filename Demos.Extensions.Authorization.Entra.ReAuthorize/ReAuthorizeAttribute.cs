using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Demos.Entra.Reauth.Shared;

[AttributeUsage(AttributeTargets.Method)]
public class ReAuthorizeAttribute : Attribute, IAsyncResourceFilter
{
    private readonly int _timeElapsedSinceLast;
    private readonly bool _requireMfa;
    public ReAuthorizeAttribute(int timeElapsedSinceLast, bool requireMfa = false)
    {
        _timeElapsedSinceLast = timeElapsedSinceLast;
        _requireMfa = requireMfa;
    }
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var foundAuthTime = int.TryParse(context.HttpContext.User.FindFirst("nbf")?.Value, out int authTime);

        if (foundAuthTime && DateTimeOffset.UtcNow.ToUnixTimeSeconds() - authTime < _timeElapsedSinceLast)
        {
            await next();
        }
        else
        {
            var state = new Dictionary<string, string?> { { "reauthenticate", "true" }, { "requiremfa", $"{_requireMfa}" } };
            await context.HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties(state)
            {
                RedirectUri = context.HttpContext.Request.Path
            });
        }
    }
}
