using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SaturnBot.Services
{
    public class LogService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;

        public LogService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;
        }
        public async Task InitializeAsync()
        {
            _discord.Log += LogDiscordMessage;
            _commands.Log += LogMessage;
        }
        public async Task LogDiscordMessage(LogMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteDiscordPrefix();
            Console.WriteLine(message.Message);
        }
        public async Task LogMessage(string message)
        {
            WriteMessage(message, LogSeverity.Info);
        }
        public async Task LogWarning(string message)
        {
            WriteMessage(message, LogSeverity.Warning);
        }
        public async Task LogMessage(LogMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            WritePrefix(message.Severity);
            Console.WriteLine(message.Message);
        }

        private void WriteMessage(string message, LogSeverity severitiy)
        {
            switch(severitiy)
            {
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Verbose:
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;                    
            }
            WritePrefix(severitiy);
            Console.WriteLine(message);
        }
        internal void WritePrefix(LogSeverity severitiy)
        {
            var timetext = DateTime.Now.ToString("yy/MM/dd HH:mm");
            var levelText = string.Format("{0,4}", severitiy);
            var text = $"[{timetext}][{levelText}]: ".ToUpper();
            Console.Write(text);
        }
        internal void WriteDiscordPrefix()
        {
            var timetext = DateTime.Now.ToString("yy/MM/dd HH:mm");
            var levelText = string.Format("{0,4}", "DNET");
            var text = $"[{timetext}][{levelText}]: ".ToUpper();
            Console.Write(text);
        }
    }
}

        
