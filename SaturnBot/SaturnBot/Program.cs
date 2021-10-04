using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.API;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SaturnBot.Services;

namespace SaturnBot
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                TotalShards = 1
            };

            Core SaturnCore = new Core();
            using (var services = SaturnCore.ConfigureServices(config))
            {

                var client = services.GetRequiredService<DiscordShardedClient>();

                client.ShardReady += SaturnCore.ReadyAsync;
                client.Log += SaturnCore.LogAsync;

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await client.LoginAsync(TokenType.Bot, SaturnCore.Configuration.BotToken);
                await client.StartAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

    }
}
