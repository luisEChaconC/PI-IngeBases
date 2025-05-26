namespace backend.Domain.Requests
{
    public class CreateEmployeeWithDependenciesRequestModel
    {
        public PersonsModel Person { get; set; }
        public UserModel User { get; set; }
        public NaturalPersonModel NaturalPerson { get; set; }
        public ContactModel Contact { get; set; }
        public EmployeeModel Employee { get; set; }
        public string EmployeeRole { get; set; }
    }
}