using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace SaturnBot
{
    public class Configuration
    {
        [JsonPropertyName("DiscordToken")]
        public string BotToken { get; set; }

        [JsonPropertyName("DBString")]
        public string DataBaseString { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }

        public Configuration()
        {
        }

        public static Configuration GetConfiguration(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Configuration>(json);
        }
        public char GetPrefix()
        {
            return Prefix[0];
        }
    }
}
