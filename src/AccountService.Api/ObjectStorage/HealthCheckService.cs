using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels.Health;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.ObjectStorage;

public class HealthCheckService(IRepository<OutboxMessage> repository, IRabbitMqConnectionMonitor monitor) : IHealthCheckService
{
    public HealthCheckViewModel CheckLiveness()
    {
        return new HealthCheckViewModel
        {
            Status = HealthStatus.Up
        };
    }

    public async Task<HealthCheckViewModel> CheckReadinessAsync()
    {
        var health = new HealthCheckViewModel
        {
            Status = HealthStatus.Up,
            Components = new List<ComponentHealthViewModel>
            {
                await GetHealthComponentRabbitMq(),
                await GetHealthComponentDatabase()
            }
        };

        if (health.Components.Any(c => c.Status == HealthStatus.Down))
        {
            health.Status = HealthStatus.Down;
        }
        else if (health.Components.Any(c => c.Status == HealthStatus.Warn))
        {
            health.Status = HealthStatus.Warn;
        }

        return health;
    }

    private async Task<ComponentHealthViewModel> GetHealthComponentDatabase()
    {
        return new ComponentHealthViewModel
        {
            Name = "Postgres",
            Status = await repository.CanConnectToDb() ? HealthStatus.Up : HealthStatus.Down
        };
    }

    private async Task<ComponentHealthViewModel> GetHealthComponentRabbitMq()
    {
        var status = HealthStatus.Up;
        
        var isWarn = false;
        
        try
        {
             isWarn = await repository
            .GetAll()
            .Where(x => x.Status == OutboxStatus.Pending || x.Status == OutboxStatus.Failed)
            .CountAsync() > 100;
        }
        catch
        {
            // ignored
        }

        if (monitor.Connection is null)
        {
            status = HealthStatus.Down;
        }
        else if (isWarn)
        {
            status = HealthStatus.Warn;
        }


        return new ComponentHealthViewModel
        {
            Name = "RabbitMq",
            Status = status
        };
    }
}
