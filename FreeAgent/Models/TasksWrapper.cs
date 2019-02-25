using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class TasksWrapper
    {
        public TasksWrapper()
        {
            tasks = new List<Task>();
        }

        public List<Task> tasks { get; set; }
    }
}