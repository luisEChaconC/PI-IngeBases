namespace backend.Models.Requests
{
    public class CreateCompanyWithDependenciesRequestModel
    {
        public PersonsModel Person { get; set; }
        public ContactModel Contact { get; set; }
        public CompanyModel Company { get; set; }
    }
}