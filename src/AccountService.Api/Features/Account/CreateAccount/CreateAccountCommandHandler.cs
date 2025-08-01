using AccountService.Api.Features.Account.Interfaces;
using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using AutoMapper;
using MediatR;

namespace AccountService.Api.Features.Account.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountStorageService accountStorageService,
    IMapper mapper,
    IClientVerificationService clientVerificationService
    ) : IRequestHandler<CreateAccountCommand, AccountViewModel>
{
    public async Task<AccountViewModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        await clientVerificationService.VerifyAsync(request.OwnerId);

        var account = mapper.Map<Domains.Account>(request);

        account.Id = Guid.NewGuid();
        account.IsDeleted = false;

        account = await accountStorageService.CreateAccountAsync(account, cancellationToken);

        var result = mapper.Map<AccountViewModel>(account);

        return result;
    }
}
