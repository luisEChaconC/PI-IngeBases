namespace backend.Domain
{
    public class EmployeeBenefitSelection
    {
        public Guid EmployeeId { get; set; }
        public List<Guid> BenefitIds { get; set; }
    }
}
