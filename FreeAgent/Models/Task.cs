using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //https://dev.freeagent.com/docs/tasks

    //https://api.freeagent.com/v2/tasks?project=xxx

    public class Task : BaseModel, IRemoveUrlOnSerialization
    {
        public string billing_period { get; set; }

        public double billing_rate { get; set; }

        public bool is_billable { get; set; }

        public string name { get; set; }

        public string project { get; set; }

        public string status { get; set; }
    }
}
