using Microservice.One.Data;
using System.Threading.Tasks;
using Microservice.One.Data.Model;

namespace Microservice.One.Services
{
    public class HsaService : IHsaService
    {
        private readonly IBenefitRepository _benefitRepository;

        public HsaService(IBenefitRepository benefitRepository)
        {
            _benefitRepository = benefitRepository;
        }

        public async Task UpsertHsa(Hsa hsa)
        {
            var existing = await _benefitRepository.GetHsaByEmployee(hsa.EmployeeId);
            if (existing == null)
            {
                await _benefitRepository.CreateHsa(hsa);
            }
            else
            {
                await _benefitRepository.UpdateHsa(hsa);
            }
        }

        public async Task ContributionAdded(int employeeId, decimal contributionAmount)
        {
            var hsa = await _benefitRepository.GetHsaByEmployee(employeeId);
            if (hsa != null)
            {
                hsa.Balance += contributionAmount;
                await _benefitRepository.UpdateHsa(hsa);
            }
        }
    }
}
