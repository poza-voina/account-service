namespace AccountService.Api.Scheduler.Interfaces;

public interface IJob
{
    Task Execute();
}