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
        public ulong ChannelId { get; set; }
        public List<StarboardMessage> Messages { get; set; }
        public int ReactionQuanitiy { get; set; }

        public Starboard(ulong id, int reactionQuanity)
        {
            ChannelId = id;
            ReactionQuanitiy = reactionQuanity;
            Messages = new List<StarboardMessage>();
        }
        private bool CheckEligibility(StarboardMessage message)
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
