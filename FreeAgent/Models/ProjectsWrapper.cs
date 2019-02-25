using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class ProjectsWrapper
    {
        public ProjectsWrapper()
        {
            projects = new List<Project>();

            //project = null;
        }

        //public Project project { get; set; }
        public List<Project> projects { get; set; }
    }
}