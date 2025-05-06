namespace backend.Models.Requests
{
    public class CreateCompanyWithDependenciesRequestModel
    {
        public PersonsModel Person { get; set; }
        public List<ContactModel> Contacts { get; set; }
        public CompanyModel Company { get; set; }
    }
}