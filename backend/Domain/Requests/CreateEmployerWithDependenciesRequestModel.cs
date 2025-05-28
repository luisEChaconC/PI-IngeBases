namespace backend.Domain.Requests
{
    public class CreateEmployerWithDependenciesRequestModel
    {
        public PersonsModel Person { get; set; }
        public UserModel User { get; set; }
        public NaturalPersonModel NaturalPerson { get; set; }
        public ContactModel Contact { get; set; }
        public EmployerModel Employer { get; set; }
    }
}