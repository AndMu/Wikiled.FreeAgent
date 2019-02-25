using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class TimeslipsWrapper
    {
        public TimeslipsWrapper()
        {
            timeslips = new List<Timeslip>();
        }

        public List<Timeslip> timeslips { get; set; }
    }
}