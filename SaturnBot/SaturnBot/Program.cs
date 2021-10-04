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
    public class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                TotalShards = 1
            };

            using (var services = ConfigureServices(config))
            {
                
                var core = services.GetService<CoreService>().GetCore();
                var client = services.GetRequiredService<DiscordShardedClient>();

                client.ShardReady += core.ReadyAsync;
                client.Log += core.LogAsync;

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await client.LoginAsync(TokenType.Bot, core.Configuration.BotToken);
                await client.StartAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }
        public ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton<CoreService>()
                .AddSingleton<CommandHandlingService>()
                .BuildServiceProvider();
        }
    }
}
