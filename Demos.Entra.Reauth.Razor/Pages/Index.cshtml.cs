using Demos.Entra.Reauth.Razor.Services;
using Demos.Entra.Reauth.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Demos.Entra.Reauth.Razor.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MessagesClient _messagesClient;

        public string? SimpleAuthMessage { get; private set; } = "No message yet.";
        public string? ReAuthMessage { get; private set; } = "No message yet.";

        public IndexModel(ILogger<IndexModel> logger, MessagesClient messagesClient)
        {
            _logger = logger;
            _messagesClient = messagesClient;
        }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            var message = await _messagesClient.GetSimpleMessage();
            SimpleAuthMessage = message;
        }

        [ReAuthorize(3)]
        public async Task OnPostReAuthAsync()
        {
            var message = await _messagesClient.GetReAuthMessage();
            ReAuthMessage = message;
        }
    }
}
