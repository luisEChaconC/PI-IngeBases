using System.ComponentModel.Design;

namespace backend.Models
{
    public class SupervisorModel
    {
        public string Id { get; set; }

        public SupervisorModel()
        {
            Id = string.Empty;
        }

        public SupervisorModel(string id)
        {
            Id = id;
        }
    }
}
