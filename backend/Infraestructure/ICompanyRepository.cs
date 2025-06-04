using backend.Domain;
using System.Collections.Generic;

namespace backend.Infraestructure
{
    public interface ICompanyRepository
    {
        void CreateCompany(CompanyModel company);
        List<CompanyModel> GetCompanies();
        CompanyModel GetCompanyById(string id);
        Task<string> GetPaymentTypeByIdAsync(Guid companyId);
    }
}