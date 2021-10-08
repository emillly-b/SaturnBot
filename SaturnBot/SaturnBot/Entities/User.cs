using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace SaturnBot.Entities
{
    public class User : Entity
    {
        public ulong DiscordId { get; set; }
        public string Username { get; set; }
        public ulong IntroMessageId { get; set; }
        public bool IsPresent { get; set; }

        public override string GenerateNewID() => DiscordId.ToString();

        public User(SocketGuildUser user)
        {
            var newuser = new User();
            newuser.DiscordId = user.Id;
            newuser.IsPresent = true;
            newuser.Username = user.Username;
        }

        public User(ulong id)
        {
            DiscordId = id;
        }

        public User()
        {
            //
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is not User) return false;
            var user = obj as User;
            return DiscordId == user.DiscordId;
        }
    }
}
