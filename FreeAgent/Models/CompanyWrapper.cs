namespace Wikiled.FreeAgent.Models
{
    public class CompanyWrapper
    {
        public CompanyWrapper()
        {
            company = new Company();
        }

        public Company company { get; set; }
    }
}