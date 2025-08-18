
namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IEventDispatcher
{
    Task DispatchAllAsync(CancellationToken cancellationToken);
}
