using Microservice.One.Data.Model;
using System.Threading.Tasks;

namespace Microservice.One.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IHsaService _hsaService;

        public PayrollService(IHsaService hsaService)
        {
            _hsaService = hsaService;
        }

        public async Task PaycheckCreated(Paycheck paycheck)
        {
            foreach (var deduction in paycheck.Deductions)
            {
                if (deduction.Type == "Hsa")
                {
                    await _hsaService.ContributionAdded(paycheck.EmployeeId, deduction.Amount);
                }
            }
        }
    }
}
