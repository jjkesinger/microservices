using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Two.Data.Model
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Salary { get; set; }
        public int Status { get; set; }

        public DateTime? LastPaycheckDate { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
