using Microservice.Shared;
using Microservice.Two.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Two.Data
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly PayrollContext _payrollContext;
        private readonly IListener _listener;
        public PayrollRepository(PayrollContext payrollContext, IListener listener)
        {
            _payrollContext = payrollContext;
            _listener = listener;
        }

        public async Task CreatePaycheck(Paycheck paycheck)
        {
            await _payrollContext.Paycheck.AddAsync(paycheck);

            await _payrollContext.SaveChangesAsync();

            await PaycheckCreated(paycheck);
        }

        public async Task<List<Employee>> GetActiveEmployeesByCompany(int companyId)
        {
            return await _payrollContext.Employee.Where(f => f.Status == 0 && f.CompanyId == companyId).ToListAsync();
        }

        public async Task<Hsa> GetHsaByEmployee(int employeeId)
        {
            return await _payrollContext.Hsa.FirstOrDefaultAsync(f => f.EmployeeId == employeeId);
        }

        public async Task CreateHsa(Hsa hsa)
        {
            await _payrollContext.AddAsync(hsa);
            await _payrollContext.SaveChangesAsync();
        }

        public async Task UpdateHsa(Hsa hsa)
        {
            _payrollContext.Update(hsa);
            await _payrollContext.SaveChangesAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _payrollContext.Update(employee);
            await _payrollContext.SaveChangesAsync();
        }

        private async Task PaycheckCreated(Paycheck paycheck)
        {
            await _listener.SendMessage(paycheck);
        }
    }
}
