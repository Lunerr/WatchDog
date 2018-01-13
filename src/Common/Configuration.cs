namespace WatchDog.Common
{
    public class Configuration
    {
        public char Prefix { get; set; }

        // TODO: Parse as Color directly!
        public uint[] Colors { get; set; }

        public uint ErrorColor { get; set; }
    }
}
