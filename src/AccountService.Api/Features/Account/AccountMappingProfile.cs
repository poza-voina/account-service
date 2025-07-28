using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.ViewModels;
using AutoMapper;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Account;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Domains.Account, AccountViewModel>();
        CreateMap<CreateAccountCommand, Domains.Account>();
    }
}
