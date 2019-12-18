using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Two.Data.Model
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

        public async Task AddDeductions(List<Deduction> deductions)
        {
            Deductions.AddRange(deductions);
            NetAmount = await CalculateNetAmount(GrossAmount, deductions);
        }

        private async Task<decimal> CalculateNetAmount(decimal grossAmount, List<Deduction> deductions)
        {
            return await Task.FromResult(grossAmount - deductions.Sum(s => s.Amount));
        }
    }
}
