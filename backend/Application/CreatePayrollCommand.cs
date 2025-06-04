using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Commands.Payroll
{
    public interface ICreatePayrollCommand
    {
        Task<Guid> ExecuteAsync(PayrollModel model);
    }

    public class CreatePayrollCommand : ICreatePayrollCommand
    {
        private readonly IPayrollRepository _repository;

        public CreatePayrollCommand(IPayrollRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(PayrollModel model)
        {
            return await _repository.CreateAsync(model);
        }
    }
}