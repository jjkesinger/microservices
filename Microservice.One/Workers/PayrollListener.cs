using Microservice.One.Data.Model;
using Microservice.One.Services;
using Microservice.Shared;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.One.Workers
{
    public class PayrollListener : BackgroundService
    {
        private readonly IListener _listener;
        private readonly IPayrollService _payrollService;

        public PayrollListener(IListener listener, IPayrollService payrollService)
        {
            _listener = listener;
            _payrollService = payrollService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() =>
            {
                _listener.RegisterListener<Paycheck>(_payrollService.PaycheckCreated);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // waiting for message
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
