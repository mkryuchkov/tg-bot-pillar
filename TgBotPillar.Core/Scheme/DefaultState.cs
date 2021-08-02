namespace TgBotPillar.Core.Scheme
{
    public static class DefaultState
    {
        public const string Start = "start";
        
        public const string Help = "help";
        
        public const string Settings = "settings";

        public static readonly string[] Commands =
        {
            $"/{Start}", $"/{Help}", $"/{Settings}"
        };
    }
}