using Microservice.One.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microservice.Shared;

namespace Microservice.One.Data
{
    public class BenefitRepository : IBenefitRepository
    {
        private readonly BenefitContext _context;
        private readonly IListener _listener;

        public BenefitRepository(BenefitContext context, IListener listener)
        {
            _context = context;
            _listener = listener;
        }

        public Task<Hsa> GetHsaByEmployee(int employeeId)
        {
            return _context.Hsa.SingleOrDefaultAsync(f => f.EmployeeId == employeeId);
        }

        public async Task CreateHsa(Hsa hsa)
        {
            await _context.AddAsync(hsa);
            await _context.SaveChangesAsync();

            await HsaUpdated(hsa);
        }

        public async Task UpdateHsa(Hsa hsa)
        {
            _context.Update(hsa);
            await _context.SaveChangesAsync();
            
            await HsaUpdated(hsa);
        }

        private async Task HsaUpdated(Hsa hsa)
        {
            await _listener.SendMessage(hsa);
        }
    }
}
