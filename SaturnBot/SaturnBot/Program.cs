using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings;
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
                var configurationService = services.GetRequiredService<ConfigurationService>();
                var client = services.GetRequiredService<DiscordShardedClient>();
                var logger = services.GetRequiredService<LogService>();
                logger.InitializeAsync();
                logger.LogMessage("Log Started");
                await configurationService.InitializeBase();

                await DB.InitAsync("saturndb", MongoClientSettings.FromConnectionString(configurationService.Configuration.DataBaseString));

                await configurationService.InitializeGlobal();
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await client.LoginAsync(TokenType.Bot, configurationService.Configuration.BotToken);
                await client.StartAsync();

                await Task.Delay(1000);
                await services.GetRequiredService<GuildHandlingService>().InitializeAsync();
                await Task.Delay(1000);
                services.GetRequiredService<ReactionHandlingService>().Initialize();
                logger.LogMessage("All Services Loaded");

                //// Let's do our global command
                //var globalCommand = new SlashCommandBuilder();
                //globalCommand.WithName("first-global-command");
                //globalCommand.WithDescription("test");
                //globalCommand.AddOption("url", ApplicationCommandOptionType.String, "test");

                //try
                //{

                //    // With global commands we dont need the guild id.
                //    await client.Rest.CreateGlobalCommand(globalCommand.Build());
                //}
                //catch (Exception exception)
                //{
                //    Console.WriteLine("Fucked up");
                //}

                await Task.Delay(Timeout.Infinite);
            }
        }
        public ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<LogService>()
                .AddSingleton<ConfigurationService>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<ReactionHandlingService>()    
                .AddSingleton<GuildHandlingService>()                
                .BuildServiceProvider();
        }
    }
}
