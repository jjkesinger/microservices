using Microservice.One.Data.Model;
using System.Threading.Tasks;

namespace Microservice.One.Services
{
    public interface IPayrollService
    {
        Task PaycheckCreated(Paycheck paycheck);
    }
}
