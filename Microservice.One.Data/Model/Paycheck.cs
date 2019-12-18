using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.One.Data.Model
{
    public class Paycheck : Entity
    {
        public Paycheck()
        {
            Deductions = new List<Deduction>();
        }

        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PayDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        public Employee Employee { get; set; }
        public List<Deduction> Deductions { get; set; }
    }
}
