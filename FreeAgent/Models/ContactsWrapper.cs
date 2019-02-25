using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class ContactsWrapper
    {
        public ContactsWrapper()
        {
            contacts = new List<Contact>();

            //contact = null;
        }

        //public Contact contact { get; set; }
        public List<Contact> contacts { get; set; }
    }
}