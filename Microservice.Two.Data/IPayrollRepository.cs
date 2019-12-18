using Microservice.Two.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Two.Data
{
    public interface IPayrollRepository
    {
        Task<List<Employee>> GetActiveEmployeesByCompany(int companyId);
        Task CreatePaycheck(Paycheck paycheck);
        Task<Hsa> GetHsaByEmployee(int employeeId);
        Task CreateHsa(Hsa hsa);
        Task UpdateHsa(Hsa hsa);
        Task UpdateEmployee(Employee employee);
    }
}
