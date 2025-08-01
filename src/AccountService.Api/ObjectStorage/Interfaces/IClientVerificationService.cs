namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IClientVerificationService
{
    Task VerifyAsync(Guid ownerId);
}
