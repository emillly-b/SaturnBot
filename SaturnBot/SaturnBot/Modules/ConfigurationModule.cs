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
using MongoDB.Entities;

namespace SaturnBot.Modules
{
    public class ConfigurationModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("saferole")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateSafeRole(string roleMention)
        {
            ulong cleanRoleId = MentionUtils.ParseRole(roleMention);
            Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).VerifiedRoleId = cleanRoleId;
            await ReplyAsync($"Safe role has been set to: {roleMention}");
        }

        [Command("unsaferole")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateUnSafeRole(string roleMention)
        {
            ulong roleId = MentionUtils.ParseRole(roleMention);
            Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).UnVerifiedRoleId = roleId;
            await ReplyAsync($"UnSafe role has been set to: {roleMention}");
        }

        [Command("setprefix")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdatePrefix(string newPrefix)
        {
            Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).Prefix = newPrefix;
            await ReplyAsync($"Prefix has been set to: `{newPrefix}`");
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateLogChannel(string channelMention)
        {
            ulong channelid = MentionUtils.ParseChannel(channelMention);
            Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).LoggingChannelId = channelid;
            await ReplyAsync($"Log channel has been set to: {channelMention}");
        }

        [Command("introchannel")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task UpdateIntroChannel(string channelMention)
        {
            ulong channelid = MentionUtils.ParseChannel(channelMention);
            Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id).IntroChannelId = channelid;
            await ReplyAsync($"Intro channel has been set to: {channelMention}");
        }

        [Command("migrateintros")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ProcessIntroChannel()
        {
            var guild = Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id);
            ulong channelid = guild.IntroChannelId;
            if (channelid == null)
            {
                await ReplyAsync($"Intro channel has not been configured. Run `{guild.Prefix}config` to show active settings");
                return;
            }
            var client = Context.Client;
            var introChannel = client.GetChannel(channelid) as ITextChannel;
            var cursor = introChannel.GetMessagesAsync(100).Flatten().GetAsyncEnumerator();
            string badIntros = "";
            while(await cursor.MoveNextAsync())
            {
                var userid = cursor.Current.Author.Id;
                if(guild.Members.Contains(new Entities.User(userid)))
                {
                    if(!cursor.Current.Author.IsBot)
                    {
                        var user = guild.Members.Find(a => a.DiscordId == userid);
                        user.IntroMessageId = cursor.Current.Id;
                    }
                    else                            
                    {
                        //
                    }                    
                }
                else
                {
                    badIntros += cursor.Current.Id + ", ";
                }                
            }
            await guild.SaveAsync();
            await ReplyAsync("Intros updated. Bad intros found: " + badIntros);
        }

        [Command("config")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task PrintConfig()
        {
            var guild = Services.GetRequiredService<GuildHandlingService>().GetGuild(Context.Guild.Id);
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = "Saturn Bot: its wiggy",
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                Description = "Barebones and to the point"
            };
            builder.WithCurrentTimestamp();
            builder.AddField("Guild settings for Guild:", $"`{guild.DiscordId}`");
            builder.AddField("Owner:", $"{MentionUtils.MentionUser(guild.OwnerId)}", inline: true);
            builder.AddField("Prefix:", $"`{guild.Prefix}`", inline: true);
            builder.AddField("Log Channel:", MentionUtils.MentionChannel(guild.LoggingChannelId), inline: true);
            builder.AddField("Safe Role:", MentionUtils.MentionRole(guild.VerifiedRoleId), inline: true);
            builder.AddField("Unsafe Role:", MentionUtils.MentionRole(guild.UnVerifiedRoleId), inline: true);
            builder.AddField("Intro Channel:", MentionUtils.MentionChannel(guild.IntroChannelId), inline: true);
            builder.AddField("Saturn Github:", "https://github.com/emillly-b/SaturnBot");
            await ReplyAsync("", embed: builder.Build());
        }
    }
}
