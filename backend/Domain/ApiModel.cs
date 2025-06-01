namespace backend.Domain
{
    public class ApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Token { get; set; }
        public string SecurityKeyName { get; set; }
        public string EndpointMethod { get; set; }
    }
}