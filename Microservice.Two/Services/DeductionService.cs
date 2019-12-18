using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public class DeductionService : IDeductionService
    {
        private readonly IPayrollRepository _payrollRepository;
        public DeductionService(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<List<Deduction>> GetDeductions(int employeeId)
        {
            var deductions = new List<Deduction>
            {
                new Deduction
                {
                    Amount = 50m,
                    Type = "Taxes"
                },
                new Deduction
                {
                    Amount = 25m,
                    Type = "401k"
                }
            };

            var hsaDeduction = await GetHsaDeduction(employeeId);
            if (hsaDeduction != null)
                deductions.Add(hsaDeduction);

            return deductions;
        }

        private async Task<Deduction> GetHsaDeduction(int employeeId)
        {
            var hsa = await _payrollRepository.GetHsaByEmployee(employeeId);

            if (hsa != null)
                return new Deduction
                {
                    Amount = hsa.ContributionAmount,
                    Type = "Hsa"
                };

            return null;
        }
    }
}
