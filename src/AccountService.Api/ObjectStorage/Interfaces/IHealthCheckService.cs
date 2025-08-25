
using AccountService.Api.ViewModels.Health;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IHealthCheckService
{
    HealthCheckViewModel CheckLiveness();
    Task<HealthCheckViewModel> CheckReadinessAsync();
}
