using Microservice.One.Data;
using Microservice.One.Services;
using Microservice.One.Workers;
using Microservice.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservice.One
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

                    services.AddDbContext<BenefitContext>(options =>
                        options.UseSqlServer(hostContext.Configuration["BenefitConnectionString"],
                            b => b.MigrationsAssembly("Microservice.One.Data")), ServiceLifetime.Singleton);

                    services.AddHostedService<PayrollListener>();

                    services.AddSingleton<IListener, Listener>();
                    services.AddTransient<IPayrollService, PayrollService>();
                    services.AddTransient<IHsaService, HsaService>();
                    services.AddTransient<IBenefitRepository, BenefitRepository>();

                    services.AddLogging();

                    var payrollContext = services.BuildServiceProvider().GetService<BenefitContext>();
                    payrollContext.Database.EnsureCreated();
                });
    }
}
