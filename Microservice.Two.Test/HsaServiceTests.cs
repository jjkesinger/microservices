using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using Microservice.Two.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Microservice.Two.Test
{
    [TestClass]
    public class HsaServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task ShouldAddHsaDeductionWhenNoneExistingForEmployee()
        {
            const int employeeId = 1;
            const decimal amount = 22.55m;

            var payrollRepository = new Mock<IPayrollRepository>();
            payrollRepository.Setup(f => f.GetHsaByEmployee(It.IsAny<int>())).ReturnsAsync((Hsa)null);

            var service = await GetHsaService(payrollRepository);

            await service.AddOrUpdateHsa(new Hsa{ EmployeeId = employeeId, ContributionAmount = amount });

            payrollRepository.Verify(f => f.CreateHsa(It.Is<Hsa>(h => h.ContributionAmount == amount && h.EmployeeId == employeeId)), Times.Once);
            payrollRepository.Verify(f => f.UpdateHsa(It.IsAny<Hsa>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ShouldUpdateHsaDeductionWhenNoneExistingForEmployee()
        {
            const int employeeId = 1;
            const decimal amount = 33.33m;

            var hsa = new Hsa
            {
                ContributionAmount = 50.00m,
                EmployeeId = employeeId,
                Id = 33
            };

            var payrollRepository = new Mock<IPayrollRepository>();
            payrollRepository.Setup(f => f.GetHsaByEmployee(It.IsAny<int>())).ReturnsAsync(hsa);

            var service = await GetHsaService(payrollRepository);

            await service.AddOrUpdateHsa(new Hsa { EmployeeId = employeeId, ContributionAmount = amount });

            payrollRepository.Verify(f => f.CreateHsa(It.IsAny<Hsa>()), Times.Never);
            payrollRepository.Verify(f => f.UpdateHsa(It.Is<Hsa>(h => h.ContributionAmount == amount && h.EmployeeId == employeeId)), Times.Once);
        }

        private async Task<HsaService> GetHsaService(Mock<IPayrollRepository> payrollRepository = null)
        {
            payrollRepository ??= new Mock<IPayrollRepository>();

            return await Task.FromResult(new HsaService(payrollRepository.Object));
        }
    }
}
