using Microservice.One.Data.Model;
using System.Threading.Tasks;

namespace Microservice.One.Data
{
    public interface IBenefitRepository
    {
        Task<Hsa> GetHsaByEmployee(int employeeId);
        Task CreateHsa(Hsa hsa);
        Task UpdateHsa(Hsa hsa);
    }
}
