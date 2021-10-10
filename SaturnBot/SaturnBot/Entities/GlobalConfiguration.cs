using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Entities;

namespace SaturnBot.Entities
{
    public class GlobalConfiguration : Entity
    {
        public override string GenerateNewID() => "globalconf";
        public string Status { get; set; }
    }
}
