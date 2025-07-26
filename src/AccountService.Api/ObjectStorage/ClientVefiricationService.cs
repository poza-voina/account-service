using AccountService.Api.Exceptions;

namespace AccountService.Api.ObjectStorage;

public interface IClientVefiricationService
{
    Task VerifyAsync(Guid ownerId);
}

public class ClientVefiricationService(ICollection<Guid> ownerIds) : IClientVefiricationService
{
    private const string OwnerNonExistsErrorMessage = "Клиент не существует";

    public Task VerifyAsync(Guid ownerId)
    {
        if (!ownerIds.Any(x => x == ownerId))
        {
            throw new UnprocessableException(OwnerNonExistsErrorMessage);
        }

        return Task.CompletedTask;
    }
}
