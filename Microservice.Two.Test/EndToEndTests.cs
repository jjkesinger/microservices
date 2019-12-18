using Microservice.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Two.Test
{
    [TestClass]
    public class EndToEndTests
    {
        [TestMethod, TestCategory("Integration"), Ignore]
        public async Task ShouldRunPayrollForCompany()
        {
            var cancellationToken = new CancellationTokenSource();

            var serviceBuilder = Program.CreateHostBuilder(null).Build();

            var listener = serviceBuilder.Services.GetService<IListener>();
            await listener.QueueMessage(1, "payroll_queue");

            var task = serviceBuilder.RunAsync(cancellationToken.Token);

            task.Wait(5000);

            cancellationToken.Cancel();
            
            //TODO verify
        }
    }
}
