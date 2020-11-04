using System;
using DSharpPlus.Entities;

namespace TCAdminNexus.Models
{
    public class DiscordMemberModel
    {
        public string Id;
        public string Username;
        public string Discriminator;
        public bool IsMuted;
        public bool IsDeafened;
        public DateTimeOffset JoinedAt;
        public DateTimeOffset CreationTimestamp;
        
        public static DiscordMemberModel Map(DiscordMember member)
        {
            return new DiscordMemberModel
            {
                Id = member.Id.ToString(),
                Username = member.Username,
                Discriminator = member.Discriminator,
                IsMuted = member.IsMuted,
                IsDeafened = member.IsDeafened,
                JoinedAt = member.JoinedAt,
                CreationTimestamp = member.CreationTimestamp
            };
        }
    }
}