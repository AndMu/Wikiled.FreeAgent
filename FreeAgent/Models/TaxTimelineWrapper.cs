using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class TaxTimelineWrapper
    {
        public TaxTimelineWrapper()
        {
            timeline_items = new List<TaxTimeline>();
        }

        public List<TaxTimeline> timeline_items { get; set; }
    }
}