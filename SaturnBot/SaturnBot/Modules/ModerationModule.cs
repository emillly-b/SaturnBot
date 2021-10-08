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
    public class ModerationModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(string user)
        {
            ulong userTokick = MentionUtils.ParseUser(user);
            await Context.Guild.GetUser(userTokick).KickAsync();
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(string user, string reason)
        {
            ulong userTokick = MentionUtils.ParseUser(user);
            await Context.Guild.GetUser(userTokick).BanAsync();
        }

        [Command("purge")]
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