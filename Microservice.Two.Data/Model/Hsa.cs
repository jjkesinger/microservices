using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Two.Data.Model
{
    public class Hsa : Entity
    {
        public int EmployeeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ContributionAmount { get; set; }
    }
}
