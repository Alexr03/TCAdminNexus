using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace TCAdminNexus.Exceptions
{
    public class CustomMessageException : Exception
    {
        public CommandContext Context;

        public CustomMessageException()
        {
        }

        public CustomMessageException(DiscordEmbed embed)
        {
            Embed = embed;
        }

        public CustomMessageException(string message)
        {
            Message = message;
        }

        public override string Message { get; } = "";

        public DiscordEmbed Embed { get; set; }

        public bool Handled { get; set; } = true;

        public virtual System.Threading.Tasks.Task DoAction()
        {
            return System.Threading.Tasks.Task.Delay(0);
        }
    }
}