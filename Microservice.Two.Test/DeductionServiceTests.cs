using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using Microservice.Two.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Two.Test
{
    [TestClass]
    public class DeductionServiceTests
    {
        [DataTestMethod, TestCategory("Unit")]
        [DataRow(1, 100)]
        [DataRow(2, 75)]
        [DataRow(3, 75)]
        public async Task DeductionsShouldIncludeHsaDeduction(int employeeId, int expectedDeduction)
        {
            var expectedDeductionTotal = decimal.Parse(expectedDeduction.ToString());
            var hsa = new List<Hsa>
            {
                new Hsa
                {
                    EmployeeId = 1,
                    ContributionAmount = 25
                },
                new Hsa
                {
                    EmployeeId = 2,
                    ContributionAmount = 0
                }
            }.FirstOrDefault(f => f.EmployeeId == employeeId);

            var payrollRepository = new Mock<IPayrollRepository>();
            payrollRepository.Setup(f => f.GetHsaByEmployee(It.IsAny<int>())).ReturnsAsync(hsa);
            var service = await GetDeductionService(payrollRepository);

            var deductions = await service.GetDeductions(employeeId);
            var calculatedDeductionTotal = deductions.Sum(f => f.Amount);

            Assert.AreEqual(expectedDeductionTotal, calculatedDeductionTotal);
        }

        private async Task<IDeductionService> GetDeductionService(Mock<IPayrollRepository> payrollRepository = null)
        {
            payrollRepository ??= new Mock<IPayrollRepository>();

            return await Task.FromResult(new DeductionService(payrollRepository.Object));
        }
    }
}
