namespace backend.Domain
{
    public class ApiParameterModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid APIId { get; set; }
    }
}