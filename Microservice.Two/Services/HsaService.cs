using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public class HsaService : IHsaService
    {
        private readonly IPayrollRepository _payrollRepository;
        public HsaService(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task AddOrUpdateHsa(Hsa hsaMessage)
        {
            var hsa = await _payrollRepository.GetHsaByEmployee(hsaMessage.EmployeeId);
            if (hsa != null)
            {
                hsa.ContributionAmount = hsaMessage.ContributionAmount;
                await _payrollRepository.UpdateHsa(hsa);
            }
            else
            {
                hsa = new Hsa
                {
                    ContributionAmount = hsaMessage.ContributionAmount,
                    EmployeeId = hsaMessage.EmployeeId
                };

                await _payrollRepository.CreateHsa(hsa);
            }
        }
    }
}
