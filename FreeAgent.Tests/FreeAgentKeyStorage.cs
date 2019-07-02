namespace Wikiled.FreeAgent.Tests
{
    public class FreeAgentKeyStorage
    {
        public static bool UseSandbox => true;

        public static bool UseProxy => false;

        public static string AppKey { get; }

        public static string AppSecret { get; }

        public static string RefreshToken { get; }
    }
}
