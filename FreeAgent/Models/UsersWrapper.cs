using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class UsersWrapper
    {
        public UsersWrapper()
        {
            users = new List<User>();
        }

        public List<User> users { get; set; }
    }
}