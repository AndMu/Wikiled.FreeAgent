using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //GET https://api.freeagent.com/v2/bank_accounts

    //https://api.freeagent.com/v2/bank_accounts

    public class BankAccount : UpdatableModel, IRemoveUrlOnSerialization
    {
        //for standard ones - account_number also on CC
        public string account_number { get; set; }

        public string bank_name { get; set; }

        public string bic { get; set; }

        public string currency { get; set; }

        public double current_balance { get; set; }

        //for paypal
        public string email { get; set; }

        public string iban { get; set; }

        public bool is_personal { get; set; }

        public string name { get; set; }

        public double opening_balance { get; set; }

        public string secondary_sort_code { get; set; }

        public string sort_code { get; set; }

        public string type { get; set; }
    }

    //needs to be moved to it's own file
}
