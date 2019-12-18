using Microservice.Two.Data.Model;
using System.Threading.Tasks;

namespace Microservice.Two.Services
{
    public interface IHsaService
    {
        Task AddOrUpdateHsa(Hsa hsa);
    }
}
