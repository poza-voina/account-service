using AccountService.Api.Scheduler.Jobs;
using Hangfire;

namespace AccountService.Api.Scheduler;

public static class JobConfigurator
{
    public static void Configure(IServiceProvider serviceProvider)
    {
        RecurringJob.AddOrUpdate<JobRunner<AccrueInterestJob>>(
            nameof(AccrueInterestJob),
            runner => runner.Run(),
            Cron.Daily);

        RecurringJob.AddOrUpdate<JobRunner<RabbitMqPublishJob>>(
            nameof(RabbitMqPublishJob),
            runner => runner.Run(),
            Cron.Monthly);
    }
}