using Microservice.Shared;
using Microservice.Two.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Two.Workers
{
    public class PayrollListener : BackgroundService
    {
        private readonly IPaycheckService _paycheckService;
        private readonly IListener _listener;
        private readonly string _payrollQueueName;

        public PayrollListener(IPaycheckService paycheckService, IListener listener,
            IOptions<AppSettings> settings)
        {
            _paycheckService = paycheckService;
            _listener = listener;
            _payrollQueueName = settings.Value.PayrollQueueName;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() =>
            {
                _listener.RegisterListener<int>(_payrollQueueName, _paycheckService.RunPayrollByCompany);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // waiting for message
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
