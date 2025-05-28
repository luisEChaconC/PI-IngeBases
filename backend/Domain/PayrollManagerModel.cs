namespace backend.Domain
{
    public class PayrollManagerModel
    {
        public string Id { get; set; }

        public PayrollManagerModel()
        {
            Id = string.Empty;
        }

        public PayrollManagerModel(string id)
        {
            Id = id;
        }
    }
}
