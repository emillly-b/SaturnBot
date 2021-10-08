using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.API;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SaturnBot.Services;
using MongoDB.Entities;
using MongoDB.Driver;

namespace SaturnBot
{
    public class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                TotalShards = 1,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All
            };

            using (var services = ConfigureServices(config))
            {
                var core = services.GetService<CoreProviderService>().GetCore();
                var client = services.GetRequiredService<DiscordShardedClient>();
                await DB.InitAsync("saturndb", MongoClientSettings.FromConnectionString(core.Configuration.DataBaseString));

                client.ShardReady += core.ReadyAsync;
                client.Log += core.LogAsync;

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();                
                services.GetRequiredService<ReactionHandlingService>().Initialize();

                await client.LoginAsync(TokenType.Bot, core.Configuration.BotToken);
                await client.StartAsync();

                await Task.Delay(1000);
                await services.GetRequiredService<GuildHandlingService>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }
        public ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<CoreProviderService>()
                .AddSingleton<ReactionHandlingService>()    
                .AddSingleton<GuildHandlingService>()
                .BuildServiceProvider();
        }
    }
}
