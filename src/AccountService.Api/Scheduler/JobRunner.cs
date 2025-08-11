using AccountService.Api.Scheduler.Interfaces;

namespace AccountService.Api.Scheduler;

public class JobRunner<TJob>(IServiceProvider serviceProvider) where TJob : IJob
{
    public void Run()
    {
        using var scope = serviceProvider.CreateScope();
        var job = scope.ServiceProvider.GetRequiredService<TJob>();
        job.Execute().GetAwaiter().GetResult();
    }
}