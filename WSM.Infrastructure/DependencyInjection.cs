
using WSM.Application.Services;
using WSM.Application.Interfaces;
using WSM.Domain.Entities;
using WSM.Domain.Entities;
using WSM.Infrastructure.Services;
using WSM.Domain.Repositories;
using WSM.Infrastructure.Repositories;
using WSM.Application.Interfaces;
using WSM.Domain.Interfaces;
using WSM.Application.Service;


namespace WSM.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TelegramBotConfig>(configuration.GetSection("TelegramBot"));
            services.AddSingleton<ITelegramBotService, TelegramBotService>();

            services.AddScoped<IMikrotikApiService, MikrotikApiService>();
            services.AddScoped<IEndpointUsageRepository, EndpointUsageRepository>();
            services.AddScoped<IEndpointCloseToExpiryRepository, EndpointCloseToExpiryRepository>();
            services.AddScoped<IMikrotikEndpointRepository, MikrotikEndpointRepository>();
            services.AddScoped<IMikrotikEndpointService, MikrotikEndpointService>();
            services.AddScoped<IWgLinuxService, WgLinuxService>();
            

            services.AddScoped<IUserRepository, UserRepository>();
            //crypto
            services.AddScoped<IWgKeyGenerationService, WgkeyGenerationService>();
            //QrGenerator
            services.AddScoped<IQrcodeGeneratorService, QrcodeGeneratorService>();
            
            
            services.AddHttpClient("MyApiClient", client =>
            {
                client.BaseAddress = new Uri("http://s2.viptls.de:505/rest/");
            });
            services.AddHostedService<EndpointUsageCollector>();
            services.AddHostedService<CheckSubscription>();

            services.AddScoped<IMikrotikCHRRepository, MikrotikCHRRepository>();
            services.AddScoped<IMikrotikCHRService, MikrotikCHRService>();
            

            services.AddLogging();

            return services;
        }
    }
}