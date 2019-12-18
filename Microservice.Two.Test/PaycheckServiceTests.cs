using Microservice.Two.Data;
using Microservice.Two.Data.Model;
using Microservice.Two.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Two.Test
{
    [TestClass]
    public class PaycheckServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task RunningPayrollForCompanyShouldRunForAllActiveEmployees()
        {
            const int companyId = 1;
            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Kesinger",
                    CompanyId = 1,
                    Id = 2,
                    Salary = 40000m,
                    Ssn = "455554545",
                    Status = 0
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    CompanyId = 1,
                    Id = 1,
                    Salary = 60000m,
                    Ssn = "455888899",
                    Status = 0
                }
            };
            
            var payrollRepository = new Mock<IPayrollRepository>();
            payrollRepository.Setup(f => f.GetActiveEmployeesByCompany(companyId))
                .ReturnsAsync(employees);

            var deductionService = new Mock<IDeductionService>();
            deductionService.Setup(f => f.GetDeductions(It.IsAny<int>()))
                .ReturnsAsync(new List<Deduction>());

            var service = await GetPaycheckService(deductionService, payrollRepository);

            await service.RunPayrollByCompany(companyId);

            payrollRepository.Verify(g => g.CreatePaycheck(It.IsAny<Paycheck>()), Times.Exactly(2));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RunningPayrollForCompanyShouldRunForAllActiveEmployeesWhoHaveNotAlreadyBeenPaidToday()
        {
            const int companyId = 1;
            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Kesinger",
                    CompanyId = 1,
                    Id = 2,
                    Salary = 40000m,
                    Ssn = "455554545",
                    Status = 0,
                    LastPaycheckDate = DateTime.Today
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    CompanyId = 1,
                    Id = 1,
                    Salary = 60000m,
                    Ssn = "455888899",
                    Status = 0
                }
            };

            var payrollRepository = new Mock<IPayrollRepository>();
            payrollRepository.Setup(f => f.GetActiveEmployeesByCompany(companyId))
                .ReturnsAsync(employees);

            var deductionService = new Mock<IDeductionService>();
            deductionService.Setup(f => f.GetDeductions(It.IsAny<int>()))
                .ReturnsAsync(new List<Deduction>());

            var service = await GetPaycheckService(deductionService, payrollRepository);

            await service.RunPayrollByCompany(companyId);

            payrollRepository.Verify(g => g.CreatePaycheck(It.IsAny<Paycheck>()), Times.Exactly(1));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task PaycheckShouldCalculateNetAmountBasedOnDeductions()
        {
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Kesinger",
                CompanyId = 1,
                Id = 2,
                Salary = 2600m,
                Ssn = "455554545",
                Status = 0
            };
            var deductions = new List<Deduction>
            {
                new Deduction
                {
                    Amount = 10,
                    Type = "Test"
                }
            };
            var expectedPaycheck = new Paycheck
            {
                Name = "John Kesinger",
                GrossAmount = 100,
                NetAmount = 90,
                EmployeeId = employee.Id,
                PayDate = DateTime.Today,
                Deductions = deductions
            };

            var deductionService = new Mock<IDeductionService>();
            deductionService.Setup(f => f.GetDeductions(It.IsAny<int>())).ReturnsAsync(deductions);

            var payrollRepository = new Mock<IPayrollRepository>();
            
            var service = await GetPaycheckService(deductionService, payrollRepository);
            await service.CreatePaycheck(employee);

            payrollRepository.Verify(f => f.CreatePaycheck(It.Is<Paycheck>(f => 
                f.Name == expectedPaycheck.Name &&
                f.GrossAmount == expectedPaycheck.GrossAmount &&
                f.NetAmount == expectedPaycheck.NetAmount && 
                f.EmployeeId == expectedPaycheck.EmployeeId &&
                f.PayDate == expectedPaycheck.PayDate)), Times.Once);
        }

        private async Task<PaycheckService> GetPaycheckService(Mock<IDeductionService> deductionService = null, Mock<IPayrollRepository> payrollRepository = null)
        {
            deductionService ??= new Mock<IDeductionService>();
            payrollRepository ??= new Mock<IPayrollRepository>();

            return await Task.FromResult(new PaycheckService(deductionService.Object, payrollRepository.Object));
        }
    }
}
