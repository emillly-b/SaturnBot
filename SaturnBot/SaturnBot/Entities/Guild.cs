using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaturnBot.Entities
{
    public class Guild
    {        
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public List<ulong> MutedUsers { get; set; }
        public ulong LoggingChannelId { get; set; }
        public ulong VerifiedRole { get; set; }
        public List<User> Members { get; set; }

        public Guild()
        {
            //
        }
    }
}
