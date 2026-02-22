using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ComponentManagement.Application.Lifetimes.Commands;

namespace ComponentManagement.Infrastructure.BackgroundJobs
{

public class LifetimeMonitoringWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LifetimeMonitoringWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new CheckLifetimeThresholdCommand(), stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

}
