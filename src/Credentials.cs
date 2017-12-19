using System.Collections.Immutable;

namespace WatchDog
{
    public class Credentials
    {
        public string Token { get; set; }
        public ImmutableArray<ulong> OwnerIds { get; set; }
    }
}
