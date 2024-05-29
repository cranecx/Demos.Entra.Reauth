using Demos.Entra.Reauth.Mvc.Models;
using Demos.Entra.Reauth.Razor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demos.Entra.Reauth.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MessagesClient _messagesClient;

        public HomeController(ILogger<HomeController> logger, MessagesClient messagesClient)
        {
            _logger = logger;
            _messagesClient = messagesClient;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel { Message = "No message yet." });
        }

        [HttpPost]
        public async Task<IActionResult> Simple()
        {
            var message = await _messagesClient.GetSimpleMessage();
            var model = new IndexViewModel { Message = message };
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> ReAuth()
        {
            var message = await _messagesClient.GetReAuthMessage();
            var model = new IndexViewModel { Message = message };
            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
