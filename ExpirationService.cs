namespace PayV;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ExpirationService : BackgroundService
{
    private readonly IServiceProvider _services;

    public ExpirationService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var temporaryLinks = scope.ServiceProvider.GetRequiredService<Dictionary<string, PaymentData>>();
                var keysOfExpiredRecords = temporaryLinks.Where(pair => DateTime.UtcNow > pair.Value.GetExpirationTime()).Select(pair => pair.Key).ToList();

                foreach (var key in keysOfExpiredRecords)
                {
                    temporaryLinks.Remove(key);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);  // Check every minute
        }
    }
}