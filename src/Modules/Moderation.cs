using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using WatchDog.Common;
using WatchDog.Extensions;
using WatchDog.Preconditions;
using WatchDog.Services;

namespace WatchDog.Modules
{
    [Name("Moderation")]
    [GuildOnly]
    // [ModOnly]
    public sealed class Moderation : ModuleBase<Context>
    {
        private readonly ModerationService _moderationService;

        public Moderation(ModerationService moderationService)
        {
            _moderationService = moderationService;
        }

        [Command("ban")]
        [Summary("Ban any user in your guild.")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IUser user, [Remainder] string reason = null)
        {
            await Context.Guild.AddBanAsync(user, 0, reason);
            await Context.ReplyAsync("You have successfully banned " + user.Tag() + ".");
        }

        [Command("kick")]
        [Summary("Kick any user from your guild.")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, [Remainder] string reason = null)
        {
            await user.KickAsync();
            await Context.ReplyAsync("You have successfully kicked " + user.Tag() + ".");
        }

        [Command("mute")]
        [Summary("Mute any user in your guild.")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Mute(IGuildUser user, [Remainder] string reason = null)
        {
            var mutedRole = await _moderationService.FetchMutedRole(Context.Guild);

            if (user.RoleIds.Contains(mutedRole.Id))
            {
                await Context.ReplyAsync("This user is already muted.");
            }
            else
            {
                await user.AddRoleAsync(mutedRole);
                await Context.ReplyAsync("You have successfully muted " + user.Tag() + ".");
            }
        }

        [Command("unmute")]
        [Summary("Unmute any muted user in your guild.")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Unmute(IGuildUser user, [Remainder] string reason = null)
        {
            var mutedRole = await _moderationService.FetchMutedRole(Context.Guild);
            if (!user.RoleIds.Contains(mutedRole.Id))
            {
                await Context.ReplyAsync("This user is not muted.");
            }
            else
            {
                await user.RemoveRoleAsync(mutedRole);
                await Context.ReplyAsync("You have successfully unmuted " + user.Tag() + ".");
            }
        }

        [Command("warn")]
        [Summary("Warn any user inside your guild.")]
        public async Task Warn(IGuildUser user, [Remainder] string reason = null)
        {
            await Context.ReplyAsync("Successfully warned " + user.Tag() + ".");
        }
    }
}
