using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Demos.Entra.Reauth.Shared;

[AttributeUsage(AttributeTargets.Method)]
public class ReAuthorizeAttribute : Attribute, IAsyncResourceFilter
{
    private readonly int _timeElapsedSinceLast;
    private readonly string _scheme;
    private readonly bool _requireMfa;
    public ReAuthorizeAttribute(int timeElapsedSinceLast, string scheme, bool requireMfa = false)
    {
        _timeElapsedSinceLast = timeElapsedSinceLast;
        _scheme = scheme;
        _requireMfa = requireMfa;
    }
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var authTimeClaim = context.HttpContext.User.FindFirst("auth_time")
            ?? context.HttpContext.User.FindFirst("iat");

        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var foundAuthTime = int.TryParse(authTimeClaim?.Value, out int authTime);

        if (foundAuthTime && (currentTime - authTime) < _timeElapsedSinceLast)
        {
            await next();
        }
        else
        {
            var state = new Dictionary<string, string?> { { "reauthenticate", "true" }, { "requiremfa", $"{_requireMfa}" } };
            await context.HttpContext.ChallengeAsync(_scheme, new AuthenticationProperties(state)
            {
                RedirectUri = context.HttpContext.Request.Path,
            });
        }
    }
}
