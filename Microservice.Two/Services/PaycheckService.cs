using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using System;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public class PaycheckService : IPaycheckService
    {
        private readonly IDeductionService _deductionService;
        private readonly IPayrollRepository _payrollRepository;
        
        public PaycheckService(IDeductionService deductionService, IPayrollRepository payrollRepository)
        {
            _deductionService = deductionService;
            _payrollRepository = payrollRepository;
        }

        public async Task RunPayrollByCompany(int companyId)
        {
            var employees = await _payrollRepository.GetActiveEmployeesByCompany(companyId);
            foreach (var employee in employees)
            {
                await CreatePaycheck(employee);
            }
        }

        public async Task CreatePaycheck(Employee employee)
        {
            if (employee.LastPaycheckDate >= DateTime.Today)
                return;

            var grossAmount = employee.Salary / 26;

            var paycheck = new Paycheck()
            {
                Name = $"{employee.FirstName} {employee.LastName}",
                EmployeeId = employee.Id,
                GrossAmount = grossAmount,
                PayDate = DateTime.Today
            };

            var deductions = await _deductionService.GetDeductions(employee.Id);
            await paycheck.AddDeductions(deductions);

            await _payrollRepository.CreatePaycheck(paycheck);

            employee.LastPaycheckDate = DateTime.Today;
            await _payrollRepository.UpdateEmployee(employee);
        }
    }
}
