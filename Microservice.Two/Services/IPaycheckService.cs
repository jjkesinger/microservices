using Microservice.Two.Data.Model;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public interface IPaycheckService
    {
        Task RunPayrollByCompany(int companyId);
        Task CreatePaycheck(Employee employee);
    }
}
