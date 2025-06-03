namespace backend.Domain
{
    public class ParameterValueModel
    {
        public Guid Id { get; set; }
        public Guid ParameterId { get; set; }
        public Guid EmployeeId { get; set; }
        public string ValueType { get; set; }
        public string? StringValue { get; set; }
        public int? IntValue { get; set; }
        public DateTime? DateValue { get; set; }
    }
}