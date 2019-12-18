using Microservice.Shared;
using Microservice.Two.Data.Model;
using Microservice.Two.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Two.Workers
{
    public class DeductionListener : BackgroundService
    {
        private readonly IHsaService _hsaService;
        private readonly IListener _listener;

        public DeductionListener(IHsaService hsaService, IListener listener)
        {
            _hsaService = hsaService;
            _listener = listener;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() =>
            {
                _listener.RegisterListener<Hsa>(_hsaService.AddOrUpdateHsa);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // waiting for message
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
