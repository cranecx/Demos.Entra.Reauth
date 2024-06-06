namespace Demos.Entra.Reauth.Mvc.Services
{
    public class MessageClientOptions
    {
        public string? BaseAddress { get; set; }
        public string[] Scopes { get; set; } = [];
    }
}
