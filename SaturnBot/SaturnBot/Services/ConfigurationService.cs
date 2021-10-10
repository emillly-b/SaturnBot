using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SaturnBot.Entities;
using MongoDB.Entities;

namespace SaturnBot.Services
{
    public class ConfigurationService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;
        private readonly LogService _log;
        public Configuration Configuration { get; set; }
        public GlobalConfiguration GlobalConfiguration { get; set; }

        public ConfigurationService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _log = services.GetRequiredService<LogService>();
            _services = services;
        }   
        public async Task InitializeBase()
        {
            try
            {
                Configuration = Configuration.GetConfiguration("./Data/Config.json");
                _log.LogMessage("Local Configuration loaded.");
            }
            catch (Exception e)
            {
                throw new Exception("Error opening config.json");
            }
        }
        public async Task InitializeGlobal()
        {
            try
            {
                var dbconf = await DB.Find<GlobalConfiguration>().OneAsync("globalconf");
                if (dbconf == null)
                    throw new Exception("Unable to load configuration from database");
                await _discord.SetGameAsync(dbconf.Status);
                _log.LogMessage("Remote Configuration loaded.");
                _log.LogMessage($"Status set too: {dbconf.Status}");

            }
            catch (Exception e)
            {
                GlobalConfiguration = new GlobalConfiguration();
                GlobalConfiguration.Status = ">help | its wiggy";
                await GlobalConfiguration.SaveAsync();
                _log.LogWarning("Database configuration was unabled to be loaded. Configuration has been reset.");
            }
        }
    }
}
