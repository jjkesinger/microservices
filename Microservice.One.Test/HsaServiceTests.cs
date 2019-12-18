using Microservice.One.Data;
using Microservice.One.Data.Model;
using Microservice.One.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Microservice.One.Test
{
    [TestClass]
    public class HsaServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task UpsertHsaShouldCreateWhenHsaDoesntExist()
        {
            var hsa = new Hsa
            {
                ContributionAmount = 50,
                EmployeeId = 2
            };

            var benefitRepository = new Mock<IBenefitRepository>();
            var service = GetHsaService(benefitRepository.Object);

            await service.UpsertHsa(hsa);

            benefitRepository.Verify(f => f.CreateHsa(It.IsAny<Hsa>()), Times.Once);
            benefitRepository.Verify(f => f.UpdateHsa(It.IsAny<Hsa>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpsertHsaShouldUpdateWhenHsaExists()
        {
            var hsa = new Hsa
            {
                ContributionAmount = 50,
                EmployeeId = 2
            };

            var benefitRepository = new Mock<IBenefitRepository>();
            benefitRepository.Setup(f => f.GetHsaByEmployee(It.IsAny<int>()))
                .ReturnsAsync(hsa);

            var service = GetHsaService(benefitRepository.Object);

            await service.UpsertHsa(hsa);

            benefitRepository.Verify(f => f.CreateHsa(It.IsAny<Hsa>()), Times.Never);
            benefitRepository.Verify(f => f.UpdateHsa(It.IsAny<Hsa>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ContributionAddedShouldUpdateHsaBalance()
        {
            var hsa = new Hsa
            {
                ContributionAmount = 19.50m,
                EmployeeId = 2,
                Balance = 10.50m
            };

            var benefitRepository = new Mock<IBenefitRepository>();
            benefitRepository.Setup(f => f.GetHsaByEmployee(It.IsAny<int>()))
                .ReturnsAsync(hsa);

            var service = GetHsaService(benefitRepository.Object);
            await service.ContributionAdded(hsa.EmployeeId, hsa.ContributionAmount);
            
            benefitRepository.Verify(f => 
                f.UpdateHsa(It.Is<Hsa>(h=>h.Balance == 30m)), Times.Once);
        }

        private HsaService GetHsaService(IBenefitRepository benefitRepository)
        {
            benefitRepository ??= new Mock<IBenefitRepository>().Object;

            return new HsaService(benefitRepository);
        }
    }
}
