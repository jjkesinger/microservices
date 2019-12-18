using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Two.Data.Model
{
    public class Deduction : Entity
    {
        public int PaycheckId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public Paycheck Paycheck { get; set; }
    }
}
