using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Entra.Reauth.Shared
{
    public static class ReAuthExtensions
    {
        public static MicrosoftIdentityOptions UseReAuthorize(this MicrosoftIdentityOptions options)
        {
            options.Events.OnRedirectToIdentityProvider = context =>
            {
                if (context.Reauthenticate())
                {
                    context.ProtocolMessage.MaxAge = "0"; // <time since last authentication or 0>;
                }
                if (context.RequireMfa())
                {
                    context.ProtocolMessage.SetParameter("amr_values", "mfa");
                }
                return Task.FromResult(0);
            };

            return options;
        }

        internal static bool Reauthenticate(this RedirectContext context)
        {
            context.Properties.Items.TryGetValue("reauthenticate", out var reauthenticate);

            bool shouldReauthenticate = false;
            if (reauthenticate != null && !bool.TryParse(reauthenticate, out shouldReauthenticate))
            {
                throw new InvalidOperationException($"'{reauthenticate}' is an invalid boolean value");
            }

            return shouldReauthenticate;
        }

        internal static bool RequireMfa(this RedirectContext context)
        {
            context.Properties.Items.TryGetValue("requiremfa", out var requiremfa);

            bool shouldRequireMfa = false;
            if (requiremfa != null && !bool.TryParse(requiremfa, out shouldRequireMfa))
            {
                throw new InvalidOperationException($"'{requiremfa}' is an invalid boolean value");
            }

            return shouldRequireMfa;
        }
    }
}
