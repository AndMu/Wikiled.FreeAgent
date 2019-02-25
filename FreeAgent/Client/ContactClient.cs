using System.Collections.Generic;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class ContactClient : ResourceClient<ContactWrapper, ContactsWrapper, Contact>
    {
        public ContactClient(FreeAgentClient client)
            : base(client)
        {
        }

        public override string ResourceName => "contacts";

        public override void CustomizeAllRequest(RestRequest request)
        {
            request.AddParameter("view", "active", ParameterType.GetOrPost);
        }

        public override List<Contact> ListFromWrapper(ContactsWrapper wrapper)
        {
            return wrapper.contacts;
        }

        public override Contact SingleFromWrapper(ContactWrapper wrapper)
        {
            return wrapper.contact;
        }

        public override ContactWrapper WrapperFromSingle(Contact single)
        {
            return new ContactWrapper {contact = single};
        }
    }
}
