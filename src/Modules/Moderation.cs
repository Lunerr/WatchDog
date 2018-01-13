using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using WatchDog.Common;

namespace WatchDog.Modules
{
    [Name("Moderation")]
    public sealed class Moderation : ModuleBase<Context>
    {
        [Command("ban")]
        [Summary("Ban any user in your guild.")]
        [RequireContext(ContextType.Guild)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IUser user, [Remainder] string reason = null)
        {
            await Context.Guild.AddBanAsync(user, 0, reason);
        }
    }
}
