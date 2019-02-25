namespace Wikiled.FreeAgent.Models
{
    public class AccessToken
    {
        public string access_token { get; set; }

        public long expires_in { get; set; }

        public string refresh_token { get; set; }

        public string token_type { get; set; }
    }
}
