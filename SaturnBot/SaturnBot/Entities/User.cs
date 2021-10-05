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
        public List<string> Usernames { get; set; }
        public List<string> Nicknames { get; set; }
        public List<string> Warnings { get; set; }
        public ulong Messages { get; set; }
        public bool IsPresent { get; set; }
    }
}
