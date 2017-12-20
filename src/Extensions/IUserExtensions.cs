using Discord;

namespace WatchDog.Extensions
{
    public static class IUserExtensions
    {
        public static string Tag(this IUser user)
        {
            return "**" + user.Username + user.Discriminator + "**";
        }
    }
}
