using Demos.Entra.Reauth.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demos.Entra.Reauth.WebAPI.Controllers
{
    [ApiController]
    [Route("messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<RetrievedMessage> GetSimpleAuthMessage()
        {
            return Ok(new RetrievedMessage
            {
                Id = Guid.NewGuid(),
                Message = "This a message retrieved from a simple-authorized endpoint.",
                DateTime = DateTime.Now,
            });
        }

        [HttpGet("reauth")]
        [ReAuthorize(3000)]
        public ActionResult<RetrievedMessage> GetReAuthMessage()
        {
            return Ok(new RetrievedMessage
            {
                Id = Guid.NewGuid(),
                Message = "This a message retrieved from a re-authorized endpoint.",
                DateTime = DateTime.Now,
            });
        }
    }
}
