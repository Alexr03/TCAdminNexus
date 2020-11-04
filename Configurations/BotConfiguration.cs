using Microsoft.Extensions.Logging;

namespace TCAdminNexus.Configurations
{
    public class BotConfiguration
    {
        public ulong ClientId { get; set; }
        public string Token { get; set; } = "";
        public string[] Prefixes { get; set; } = {";"};
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;
    }
}