using backend.Infraestructure;

namespace backend.Application.Commands.Company
{
    public interface IDeleteCompanyCommand
    {
        void Execute(string companyId);
    }

    public class DeleteCompanyCommand : IDeleteCompanyCommand
    {
        private readonly ICompanyRepository _repository;

        public DeleteCompanyCommand(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public void Execute(string companyId)
        {
            if (companyId == null)
                throw new ArgumentNullException(nameof(companyId));

            if (string.IsNullOrWhiteSpace(companyId))
                throw new ArgumentException("Company ID cannot be empty", nameof(companyId));

            _repository.DeleteCompany(companyId);
        }

    }
}