namespace Wikiled.FreeAgent.Models
{
    public class TaxTimeline
    {
        public float amount_due { get; set; }

        public string dated_on { get; set; }

        public string description { get; set; }

        public bool is_personal { get; set; }

        public string nature { get; set; }
    }
}