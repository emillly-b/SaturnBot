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
    public class GuildHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;
        public List<Guild> ActiveGuilds;

        public GuildHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;
            _discord.GuildAvailable += GuildAvailable;
            _discord.UserJoined += UserJoined;
            _discord.UserLeft += UserLeft;
        }
        public async Task InitializeAsync()
        {
            ActiveGuilds = new List<Guild>();
            var guilds = await DB.Find<Guild>().ManyAsync(a => true);
            foreach(Guild g in guilds)
            {
                ActiveGuilds.Add(g);
            }
            foreach (SocketGuild sockGuild in _discord.Guilds)
            {
                var guild = new Guild(sockGuild);
                if (ActiveGuilds.Contains(guild))
                    break;
                ActiveGuilds.Add(guild);
                await guild.SaveAsync();
            }
        }
        public Guild GetGuild(ulong id)
        {
            return ActiveGuilds.Find(x => x.DiscordId == id);
        }
        public async Task SaveGuildsAsync()
        {
            foreach (Guild g in ActiveGuilds)
                await g.SaveAsync();
        }

        private async Task GuildAvailable(SocketGuild arg)
        {
            //   
        }
        private async Task GuildMemberUpdated(Cacheable<SocketGuildUser,ulong> userBefore, SocketGuildUser userAfter)
        {
            Guild guild = GetGuild(userAfter.Guild.Id);
            var embed = new EmbedBuilder()
                .WithTitle($"User Updated :{userAfter.Username}");
            if(userAfter is not SocketGuildUser)
            {
                embed.AddField("Error", "Unable to determine what has changed.");
                var channel = _discord.GetChannel(guild.LoggingChannelId) as ITextChannel;
                await channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            var context = GetGuild(user.Guild.Id);
            context.Members.Add(new User(user));
            var embed = new EmbedBuilder()
                        .WithColor(Color.Blue)
                        .WithDescription("User Joined:" + MentionUtils.MentionUser(user.Id))
                        .WithThumbnailUrl(user.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithAuthor(_discord.CurrentUser);
            embed.AddField("ID: ", user.Id, inline: true);
            embed.AddField("Account Age", user.CreatedAt.ToString(), inline: true);
            embed.AddField("User Status", user.Status.ToString());
            var joinedEmbed = embed.Build();
            var log = _discord.GetChannel(context.LoggingChannelId) as ITextChannel;
            await log.SendMessageAsync("", embed: joinedEmbed);
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            var context = GetGuild(user.Guild.Id);
            context.Members.Add(new User(user));
            var embed = new EmbedBuilder()
                        .WithColor(Color.Blue)
                        .WithDescription("User Left:" + MentionUtils.MentionUser(user.Id))
                        .WithThumbnailUrl(user.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithAuthor(_discord.CurrentUser);
            embed.AddField("ID: ", user.Id, inline: true);
            embed.AddField("Account Age", user.CreatedAt.ToString(), inline: true);
            embed.AddField("Role Count", user.Roles.Count, inline: true);
            var leftEmbed = embed.Build();
            var log = _discord.GetChannel(context.LoggingChannelId) as ITextChannel;
            await log.SendMessageAsync("", embed: leftEmbed);
        }
    }
}
