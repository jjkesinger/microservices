using Microservice.Shared;
using Microservice.Two.Data;
using Microservice.Two.Services;
using Microservice.Two.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microservice.Two
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var sbConfiguration = hostContext.Configuration.GetSection("AppSettings");
                    services.Configure<AppSettings>(sbConfiguration);

                    services.AddMemoryCache();

                    services.AddDbContext<PayrollContext>(options =>
                        options.UseSqlServer(hostContext.Configuration["PayrollConnectionString"],
                            b => b.MigrationsAssembly("Microservice.Two.Data")), ServiceLifetime.Singleton);

                    services.AddHostedService<DeductionListener>();
                    services.AddHostedService<PayrollListener>();

                    services.AddSingleton<IListener, Listener>();
                    services.AddTransient<IPaycheckService, PaycheckService>();
                    services.AddTransient<IPayrollRepository, PayrollRepository>();
                    services.AddTransient<IDeductionService, DeductionService>();
                    services.AddTransient<IHsaService, HsaService>();

                    services.AddLogging(f => f.SetMinimumLevel(LogLevel.Information));

                    var payrollContext = services.BuildServiceProvider().GetService<PayrollContext>();
                    payrollContext.Database.EnsureCreated();
                });
    }
}
