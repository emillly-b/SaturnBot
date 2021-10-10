using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Entities;
using MongoDB;
using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace SaturnBot.Entities
{
    public class Guild : Entity
    {
        public ulong DiscordId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public ulong OwnerId { get; set; }
        public ulong LoggingChannelId { get; set; }
        public ulong IntroChannelId { get; set; }
        public ulong VerifiedRoleId { get; set; }
        public ulong UnVerifiedRoleId { get; set; }
        public List<User> Members { get; set; }
        public override string GenerateNewID() => DiscordId.ToString();
        public override string ToString() => Name;
        public override bool Equals(object obj)
        {            
            if (obj is not Guild) return false;
            var guild = (Guild)obj;
            if (DiscordId == guild.DiscordId) 
                return true;
            return false;
        }
        public Guild(SocketGuild socketGuild)
        {
            Members = new List<User>();
            DiscordId = socketGuild.Id;
            OwnerId = socketGuild.OwnerId;
            Prefix = ">";
            Name = socketGuild.Name;
            var members = socketGuild.Users.ToList();
            foreach(SocketGuildUser user in members)
            {
                Members.Add(new User(user));
            }
        }
    }
}
