
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WSM.Application.Interfaces;
using WSM.Domain.Repositories;

namespace WSM.Application.Services
{
    public class EndpointUsageCollector : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EndpointUsageCollector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    var repositoryEndpoint = scope.ServiceProvider.GetRequiredService<IEndpointUsageRepository>();
                    var repositoryCHR = scope.ServiceProvider.GetRequiredService<IMikrotikCHRRepository>();
                    var mikrotikApiService = scope.ServiceProvider.GetRequiredService<IMikrotikApiService>();

                    var mikrotikChrs = await repositoryCHR.GetMikrotkCHRs();
                    foreach (var chr in mikrotikChrs)
                    {
                        var response = await mikrotikApiService.MikrotikApiFetch(chr,"rest/interface/wireguard/peers");
                        await repositoryEndpoint.InsertAll(response.Data, chr.IpAddress);
                    }



                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
                }
            }
        }
    }
}