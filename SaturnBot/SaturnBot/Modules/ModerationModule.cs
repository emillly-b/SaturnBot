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
        [Command("kick")]
        public async Task KickAsync(string user)
        {
            //teehee.
            if (Context.Message.Author.Id != 341275030941859850)
                return;
            ulong userTokick = MentionUtils.ParseUser(user);
            await Context.Guild.GetUser(userTokick).KickAsync();
        }
        [Command("ban")]
        public async Task BanAsync(string user, string reason)
        {
            //teehee.
            if (Context.Message.Author.Id != 341275030941859850)
                return;
            ulong userTokick = MentionUtils.ParseUser(user);
            await Context.Guild.GetUser(userTokick).BanAsync();
        }
        [Command("purge")]
        public async Task PurgeAsync(int PurgeAmount)
        {
            //teehee.
            if (Context.Message.Author.Id != 341275030941859850)
                return;
            var msgs = await Context.Channel.GetMessagesAsync(PurgeAmount).FlattenAsync();
            var channel = (SocketTextChannel) Context.Channel;
            await channel.DeleteMessagesAsync(msgs);
        }
    }
}
