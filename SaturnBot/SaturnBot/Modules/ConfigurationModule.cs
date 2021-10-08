using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using SaturnBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SaturnBot.Modules
{
    public class ConfigurationModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider _services { get; set; }

        [Command("saferole")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateSafeRole(string roleMention)
        {
            ulong cleanRoleId = MentionUtils.ParseRole(roleMention);
            _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).VerifiedRoleId = cleanRoleId;
            await ReplyAsync($"Safe role has been set to: {roleMention}");
        }

        [Command("unsaferole")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateUnSafeRole(string roleMention)
        {
            ulong roleId = MentionUtils.ParseRole(roleMention);
            _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).UnVerifiedRoleId = roleId;
            await ReplyAsync($"UnSafe role has been set to: {roleMention}");
        }

        [Command("setprefix")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdatePrefix(string newPrefix)
        {
            _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).Prefix = newPrefix;
            await ReplyAsync($"Prefix has been set to: `{newPrefix}`");
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateLogChannel(string channelMention)
        {
            ulong channelid = MentionUtils.ParseChannel(channelMention);
            _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).LoggingChannelId = channelid;
            await ReplyAsync($"Log channel has been set to: {channelMention}");
        }

        [Command("introchannel")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateIntroChannel(string channelMention)
        {
            ulong channelid = MentionUtils.ParseChannel(channelMention);
            _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).IntroChannelId = channelid;
            await ReplyAsync($"Intro channel has been set to: {channelMention}");
        }

        [Command("config")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task PrintConfig()
        {
            var guild = _services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id);
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = "Saturn Bot: its wiggy",
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                Description = "Barebones and to the point"
            };
            builder.WithCurrentTimestamp();
            builder.AddField("Github", "https://github.com/emillly-b/SaturnBot");
            builder.AddField("Guild settings for Guild:", $"`{guild.Id}`");
            builder.AddField("Owner:", $"`{MentionUtils.MentionUser(guild.Id)}`", inline: true);
            builder.AddField("Prefix:", $"`{guild.Prefix}`", inline: true);
            builder.AddField("Log Channel:", MentionUtils.MentionChannel(guild.LoggingChannelId), inline: true);
            builder.AddField("Safe Role:", MentionUtils.MentionRole(guild.VerifiedRoleId), inline: true);
            builder.AddField("Unsafe Role:", MentionUtils.MentionRole(guild.UnVerifiedRoleId), inline: true);
            builder.AddField("Intro Channel:", MentionUtils.MentionChannel(guild.IntroChannelId), inline: true);
            await ReplyAsync("", embed: builder.Build());
        }
    }
}
