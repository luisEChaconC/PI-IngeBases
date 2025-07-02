using backend.Infraestructure;

namespace backend.Application.Commands.Employee
{
    public interface IDeleteEmployeeCommand
    {
        void Execute(string employeeId);
    }

    public class DeleteEmployeeCommand : IDeleteEmployeeCommand
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeCommand(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public void Execute(string employeeId)
        {
            if (employeeId == null)
                throw new ArgumentNullException(nameof(employeeId));

            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            _repository.DeleteEmployee(employeeId);
        }
    }
}
