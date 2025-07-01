using backend.Domain;
using backend.Application.DTOs;
namespace backend.Infraestructure
{
    public interface IBenefitRepository
    {
        List<Benefit> GetBenefits(string companyId);
        List<Benefit> GetAssignedBenefitsForEmployee(Guid employeeId);
        bool CreateBenefit(Benefit benefit);
        bool UpdateBenefit(Benefit benefit);
        DeleteBenefitDto DeleteBenefit(Guid benefitId);
        bool AssignBenefitsToEmployee(Guid employeeId, List<Guid> benefitIds);
        bool IsBenefitAssignedToAnyEmployee(Guid benefitId);
        Benefit? GetBenefitById(Guid id);
        bool DisableBenefitForEmployee(Guid benefitId, Guid employeeId);
    }
}