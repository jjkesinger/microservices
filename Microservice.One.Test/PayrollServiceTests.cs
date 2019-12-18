using Microservice.One.Data.Model;
using Microservice.One.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.One.Test
{
    [TestClass]
    public class PayrollServiceTests
    {
        [TestMethod]
        public async Task PaycheckCreatedWithHsaDeductionShouldAddContribution()
        {
            var paycheck = new Paycheck
            {
                EmployeeId = 3,
                Deductions = new List<Deduction>
                {
                    new Deduction
                    {
                        Type = "Hsa",
                        Amount = 40m
                    },
                    new Deduction
                    {
                        Type = "401k",
                        Amount = 20m
                    }
                }
            };
            
            var hsaService = new Mock<IHsaService>();
            var service = GetPayrollService(hsaService.Object);

            await service.PaycheckCreated(paycheck);

            hsaService.Verify(f=>f.ContributionAdded(paycheck.EmployeeId, 40m));
        }

        private PayrollService GetPayrollService(IHsaService hsaService = null)
        {
            hsaService ??= new Mock<IHsaService>().Object;

            return new PayrollService(hsaService);
        }
    }
}
