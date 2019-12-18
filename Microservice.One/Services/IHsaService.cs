using Microservice.One.Data.Model;
using System.Threading.Tasks;

namespace Microservice.One.Services
{
    public interface IHsaService
    {
        Task ContributionAdded(int employeeId, decimal contributionAmount);
        Task UpsertHsa(Hsa hsa);
    }
}
