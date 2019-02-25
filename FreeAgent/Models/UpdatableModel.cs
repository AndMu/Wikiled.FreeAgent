namespace Wikiled.FreeAgent.Models
{
    public class UpdatableModel : BaseModel
    {
        public UpdatableModel()
        {
            updated_at = "";
            created_at = "";
        }

        public string created_at { get; set; }

        public string updated_at { get; set; }
    }
}