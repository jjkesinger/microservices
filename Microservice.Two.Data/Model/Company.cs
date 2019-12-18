using System.Collections.Generic;

namespace Microservice.Two.Data.Model
{
    public class Company : Entity
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
