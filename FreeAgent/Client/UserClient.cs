using System.Collections.Generic;
using System.Threading.Tasks;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class UserClient : ResourceClient<UserWrapper, UsersWrapper, User>
    {
        public UserClient(FreeAgentClient client)
            : base(client)
        {
        }

        //need to add in the GET to have a parameter for the date filter

        public override string ResourceName => "users";

        public Task<User> ResolveMe() => Get("me");

        public override List<User> ListFromWrapper(UsersWrapper wrapper)
        {
            return wrapper.users;
        }

        public override User SingleFromWrapper(UserWrapper wrapper)
        {
            return wrapper.user;
        }

        public override UserWrapper WrapperFromSingle(User single)
        {
            return new UserWrapper {user = single};
        }
    }
}
