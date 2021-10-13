using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

namespace SaturnBot.Services
{
    public class SlashCommandService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;

        public SlashCommandService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;


        }

        public async Task UnregisterCommands()
        {
            var commands = await _discord.Rest.GetGlobalApplicationCommands();
            if (commands == null)
                return;
            foreach (RestGlobalCommand cmd in commands)
                cmd.DeleteAsync();
        }

        public async Task RegisterCommands()
        {
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("boo");
            globalCommand.WithDescription("spooky");
            await _discord.Rest.CreateGuildCommand(globalCommand.Build(), 868546771406692392);
        }

        public async Task InitializeAsync()
        {
            await UnregisterCommands();
            await RegisterCommands();
            _discord.InteractionCreated += HandleInteraction;
        }

        public async Task HandleInteraction(SocketInteraction interaction)
        {
            if (interaction is not SocketSlashCommand)
                return;
            await interaction.RespondAsync("Ooh!");
        }
    }
}
