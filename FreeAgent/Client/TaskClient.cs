using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Models;
using Task = Wikiled.FreeAgent.Models.Task;

namespace Wikiled.FreeAgent.Client
{
    public class TaskClient : ResourceClient<TaskWrapper, TasksWrapper, Task>
    {
        public TaskClient(FreeAgentClient client)
            : base(client)
        {
        }

        //need to add in the GET to have a parameter for the project

        public override string ResourceName => "tasks";

        public override List<Task> ListFromWrapper(TasksWrapper wrapper)
        {
            return wrapper.tasks;
        }

        public override Task SingleFromWrapper(TaskWrapper wrapper)
        {
            return wrapper.task;
        }

        public override TaskWrapper WrapperFromSingle(Task single)
        {
            return new TaskWrapper {task = single};
        }

        public IObservable<Task> AllByProject(string projectId)
        {
            return All(delegate(RestRequest req) { req.AddParameter("project", projectId, ParameterType.GetOrPost); });
        }

        public async Task<Task> Put(Task item, string projectId)
        {
            var request = CreatePutRequest(item);
            request.Resource += "?project={project}";
            request.AddParameter("project", projectId, ParameterType.UrlSegment);
            var response = await Client.Execute<TaskWrapper>(request).ConfigureAwait(false);
            return response != null ? SingleFromWrapper(response) : null;
        }
    }
}
