using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Entities;

namespace SaturnBot.Entities
{
    public class Starboard : Entity
    {
        public bool IsEnabled { get; set; }
        public ulong ChannelId { get; set; }
        public int ReactionQuanitiy { get; set; }
        public List<StarboardMessage> Messages { get; set; }

        public Starboard()
        {
            IsEnabled = false;
            ChannelId = 0;
            ReactionQuanitiy = 3;
            Messages = new List<StarboardMessage>();

        }
        public Starboard(ulong id, int reactionQuanity)
        {
            IsEnabled = true;
            ChannelId = id;
            ReactionQuanitiy = reactionQuanity;
            Messages = new List<StarboardMessage>();
        }
        public bool CheckMessageEligibility(StarboardMessage message)
        {
            return message.StarCount > ReactionQuanitiy;
        }
    }

    public class StarboardMessage : Entity
    {
        public ulong Id { get; set; }
        public int StarCount { get; set; }        
    }
}
