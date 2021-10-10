using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using SaturnBot.Services;
using MongoDB.Entities;

namespace SaturnBot.Modules
{
    [Remarks("Moderation Commands")]
    public class ModerationModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("kick")]
        [Remarks("Removes the user from the server.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(string user)
        {
            if (MentionUtils.TryParseUser(user, out ulong uid))
                await Context.Guild.GetUser(uid).KickAsync();
            else
                await Context.Guild.GetUser(ulong.Parse(user)).KickAsync();
        }

        [Command("ban")]
        [Remarks("Bans the user from the server without removing posts.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(string user, string reason)
        {
            if (MentionUtils.TryParseUser(user, out ulong uid))
                await Context.Guild.GetUser(uid).BanAsync(reason: reason);
            else
                await Context.Guild.GetUser(ulong.Parse(user)).BanAsync(reason: reason);
        }

        [Command("nuke")]
        [Remarks("Bans the user and removes all posts made by the user in the last day")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task NukeAsync(string user, string reason)
        {
            if (MentionUtils.TryParseUser(user, out ulong uid))
                await Context.Guild.GetUser(uid).BanAsync(reason: reason, pruneDays: 1);
            else
                await Context.Guild.GetUser(ulong.Parse(user)).BanAsync(reason: reason, pruneDays: 1);
        }

        [Command("purge")]
        [Remarks("Deletes specified amount of messages from the current channel")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int PurgeAmount)
        {
            if (Context.Channel is SocketDMChannel)
            {
                await Context.Message.ReplyAsync("This command can only be used in a guild channel.");
                return;
            }
            var msgs = await Context.Channel.GetMessagesAsync(PurgeAmount).FlattenAsync();
            var channel = (SocketTextChannel)Context.Channel;
            await channel.DeleteMessagesAsync(msgs);
        }

        [Command("welcome")]
        [Remarks("Welcomes a user into the server and cross posts their intro to the safe channel")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task WelcomeAsync(string input)
        {
            var userid = MentionUtils.ParseUser(input);
            var messages = Context.Channel.GetMessagesAsync().Flatten().GetAsyncEnumerator();
            var user = Context.Guild.GetUser(userid);
            var guild = Services.GetRequiredService<GuildHandlingService>().ActiveGuilds.Find(a => a.DiscordId == Context.Guild.Id);
            await user.AddRoleAsync(guild.VerifiedRoleId);
            await user.RemoveRoleAsync(guild.UnVerifiedRoleId);
            
            while (await messages.MoveNextAsync())
            {
                if(messages.Current.Author.Id == userid)
                {
                    var embed = new EmbedBuilder()
                        .WithColor(Color.Blue)
                        .WithDescription("Welcome " + MentionUtils.MentionUser(userid))
                        .WithThumbnailUrl(messages.Current.Author.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithAuthor(messages.Current.Author)
                        .WithTitle($"{messages.Current.Author.Username}'s Introduction!");
                    embed.AddField("Intro:", messages.Current.Content);
                    var introEmbed = embed.Build();
                    var safeChannel = (ISocketMessageChannel) Context.Guild.GetChannel(guild.IntroChannelId);
                    var introMessage = await safeChannel.SendMessageAsync("", embed: introEmbed);
                    var dbUser = guild.Members.Find(a => a.DiscordId == userid);
                    dbUser.IntroMessageId = introMessage.Id;
                    await dbUser.SaveAsync();
                    await messages.Current.DeleteAsync();                    
                    break;
                }
            }            
            await Context.Message.DeleteAsync();
        }
    }
}