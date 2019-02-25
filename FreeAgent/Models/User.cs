using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //https://dev.freeagent.com/docs/users

    //https://api.freeagent.com/v2/users
    public class User : UpdatableModel, IRemoveUrlOnSerialization
    {
        public string email { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public double opening_mileage { get; set; }

        public string password { get; set; }

        public string password_confirmation { get; set; }

        public int permission_level { get; set; }

        public string role { get; set; }
    }
}
