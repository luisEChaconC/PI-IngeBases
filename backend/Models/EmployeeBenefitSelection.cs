namespace backend.Models
{
    public class EmployeeBenefitSelection
    {
        public Guid EmployeeId { get; set; }
        public List<Guid> BenefitIds { get; set; }
    }
}
