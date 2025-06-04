using backend.Domain;

namespace backend.Infraestructure
{
    public interface IBenefitRepository
    {
        List<Benefit> GetBenefits(string companyId);
        bool AssignBenefitsToEmployee(Guid employeeId, List<Guid> benefitIds);
        List<Benefit> GetAssignedBenefitsForEmployee(Guid employeeId);
        bool CreateBenefit(Benefit benefit);
        bool DisableBenefitForEmployee(Guid benefitId, Guid employeeId);
    }
}