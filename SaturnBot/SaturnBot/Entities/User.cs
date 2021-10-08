using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaturnBot.Entities
{
    public class User
    {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public ulong IntroMessageId { get; set; }
        public bool IsPresent { get; set; }
    }
}
