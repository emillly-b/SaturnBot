using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SaturnBot.Entities
{
    public class Guild
    {        
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public ulong OwnerId { get; set; }
        public ulong LoggingChannelId { get; set; }
        public ulong IntroChannelId { get; set; }
        public ulong VerifiedRoleId { get; set; }
        public ulong UnVerifiedRoleId { get; set; }
        public List<User> Members { get; set; }

        public Guild(SocketGuild socketGuild)
        {
            Members = new List<User>();
            Id = socketGuild.Id;
            var members = socketGuild.Users.GetEnumerator();
            while(members.MoveNext())
            {
                var newuser = new User();
                newuser.Id = members.Current.Id;
                newuser.IsPresent = true;
                newuser.Username = members.Current.Username;
                Members.Add(newuser);
            }
            OwnerId = socketGuild.OwnerId;
        }
    }
}
