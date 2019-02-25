namespace Wikiled.FreeAgent.Models
{
    //https://dev.freeagent.com/docs/timeslips

    //https://api.freeagent.com/v2/timeslips?from_date=?&to_date=?

    public class Timeslip : UpdatableModel
    {
        public string comment { get; set; }

        public string dated_on { get; set; }

        public double hours { get; set; }

        public string project { get; set; }

        public string task { get; set; }

        public string user { get; set; }
    }
}
