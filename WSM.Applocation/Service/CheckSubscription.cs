using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Globalization;
using WSM.Domain.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using WSM.Application.Interfaces;
namespace WSM.Application.Services
{
    public class CheckSubscription : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITelegramBotService _botService;
        public CheckSubscription(ITelegramBotService botService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _botService = botService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    //var repositoryEndpoint = scope.ServiceProvider.GetRequiredService<IEndpointUsageRepository>();
                    var repository = scope.ServiceProvider.GetRequiredService<IEndpointCloseToExpiryRepository>();
                    //var mikrotikApiService = scope.ServiceProvider.GetRequiredService<IMikrotikApiService>();

                    var closeToExpire = await repository.GetItemsCloseToExpire(TimeSpan.FromDays(3));
                    if (closeToExpire != null)
                    {
                        foreach (var ep in closeToExpire)
                        {
                            if (ep.EndDate.HasValue)
                            {
                                var endDate = ep.EndDate.Value;
                                var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                                var endDateInTimeZone = TimeZoneInfo.ConvertTime(endDate, timeZone);

                                var persianCalendar = new PersianCalendar();                        
                                var shamsiDate = string.Format("{0}/{1}/{2} {3}:{4}",
                                    persianCalendar.GetYear(endDateInTimeZone),
                                    persianCalendar.GetMonth(endDateInTimeZone),
                                    persianCalendar.GetDayOfMonth(endDateInTimeZone),
                                    endDateInTimeZone.Hour,
                                    endDateInTimeZone.Minute
                                    );
                                _botService.SendMessage(ep.TelegramId,
                                        string.Format("account {0} from {1} will be expired on {2} با تشکر", ep.AllowedAddress, ep.MikrotikInterface, shamsiDate));


                            }
                        }
                    }
                }


                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
