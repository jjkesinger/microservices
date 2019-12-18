using Microservice.Two.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public interface IDeductionService
    {
        Task<List<Deduction>> GetDeductions(int employeeId);
    }
}
