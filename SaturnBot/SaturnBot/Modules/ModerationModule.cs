using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace SaturnBot.Modules
{
    public class ModerationModule : ModuleBase<ShardedCommandContext>
    {
        ulong SafeRoleId = 868547121933058067;
        ulong UnsafeRoleId = 868667796870021201;
        ulong SafeChannelId = 868551387624120340;


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
            while(await messages.MoveNextAsync())
            {
                if(messages.Current.Author.Id == userid)
                {
                    var embed = new EmbedBuilder()
                        .WithColor(Color.LightOrange)
                        .WithDescription("Welcome " + MentionUtils.MentionUser(userid))
                        .WithThumbnailUrl(messages.Current.Author.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithAuthor(messages.Current.Author)
                        .WithTitle($"{messages.Current.Author.Username}'s Introduction!");
                    embed.AddField("Intro:", messages.Current.Content);
                    var introEmbed = embed.Build();
                    var safeChannel = (ISocketMessageChannel) Context.Guild.GetChannel(SafeChannelId);
                    await safeChannel.SendMessageAsync("", embed: introEmbed);
                    await messages.Current.DeleteAsync();
                    break;
                }
            }
            var user = Context.Guild.GetUser(userid);
            await user.AddRoleAsync(SafeRoleId);
            await user.RemoveRoleAsync(UnsafeRoleId);
            await Context.Message.DeleteAsync();
        }
    }
}