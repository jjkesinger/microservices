namespace Microservice.One.Data.Model
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
