using Newtonsoft.Json;

namespace Wikiled.FreeAgent.Models
{
    public class ExpenseAttachment
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("content_src")]
        public string ContentSrc { get; set; }

        [JsonProperty("content_src_medium")]
        public string ContentSrcMedium { get; set; }

        [JsonProperty("content_src_small")]
        public string ContentSrcSmall { get; set; }

        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

    }
}