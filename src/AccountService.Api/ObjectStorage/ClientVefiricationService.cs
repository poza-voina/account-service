using AccountService.Abstractions.Exceptions;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class ClientVerificationService(ICollection<Guid> ownerIds) : IClientVerificationService
{
    public ICollection<Guid> OwnerIds { get; set; } = ownerIds;
    private const string OwnerNonExistsErrorMessage = "Клиент не существует";

    public Task VerifyAsync(Guid ownerId)
    {
        if (OwnerIds.All(x => x != ownerId))
        {
            throw new UnprocessableException(OwnerNonExistsErrorMessage);
        }

        return Task.CompletedTask;
    }
}
