namespace Wikiled.FreeAgent.Models
{
    public class BankTransaction : UpdatableModel
    {
        public double amount { get; set; }

        public string bank_account { get; set; }

        public string dated_on { get; set; }

        public string description { get; set; }

        public bool is_manual { get; set; }

        public double unexplained_amount { get; set; }
    }
}