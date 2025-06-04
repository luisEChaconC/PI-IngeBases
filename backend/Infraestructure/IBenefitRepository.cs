using backend.Domain;

namespace backend.Infraestructure
{
    public interface IBenefitRepository
    {
        List<Benefit> GetBenefits(string companyId);
        List<Benefit> GetAssignedBenefitsForEmployee(Guid employeeId);
        bool CreateBenefit(Benefit benefit);
        bool UpdateBenefit(Benefit benefit);
        bool AssignBenefitsToEmployee(Guid employeeId, List<Guid> benefitIds);
        bool IsBenefitAssignedToAnyEmployee(Guid benefitId);
        Benefit? GetBenefitById(Guid id);
        bool DisableBenefitForEmployee(Guid benefitId, Guid employeeId);
    }
}