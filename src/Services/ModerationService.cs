using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace WatchDog.Services
{
    public sealed class ModerationService
    {
        public async Task<IRole> FetchMutedRole(IGuild guild)
        {
            var muted = guild.Roles.FirstOrDefault((x) => x.Name == "Muted");

            if (muted == default(IRole))
            {
                muted = await guild.CreateRoleAsync("Muted");

                var textChannels = (await guild.GetChannelsAsync()).Where((x) => x is ITextChannel);
                var permissions = new OverwritePermissions(PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Deny);

                foreach (var channel in textChannels)
                {
                    // TODO: Add Permission overwrites inside text channel creation event
                    await channel.AddPermissionOverwriteAsync(muted, permissions);
                }
            }

            return muted;
        }
    }
}
