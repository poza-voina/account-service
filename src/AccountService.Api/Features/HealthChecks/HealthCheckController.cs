using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels.Health;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Features.HealthChecks;

[ApiController]
[AllowAnonymous]
[Route("health")]
public class HealthController(IHealthCheckService healthCheckService) : ControllerBase
{
    /// <summary>
    /// Проверка живости сервиса
    /// </summary>
    [HttpGet("live")]
    [ProducesResponseType(typeof(HealthCheckViewModel), 200)]
    public ActionResult<HealthCheckViewModel> Live()
    {
        var result = healthCheckService.CheckLiveness();
        return Ok(result);
    }

    /// <summary>
    /// Проверка готовности сервиса
    /// </summary>
    [HttpGet("ready")]
    [ProducesResponseType(typeof(HealthCheckViewModel), 200)]
    public async Task<ActionResult<HealthCheckViewModel>> Ready()
    {
        var result = await healthCheckService.CheckReadinessAsync();

        return Ok(result);
    }
}