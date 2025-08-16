using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.ViewModels;
using AutoMapper;
using Models = AccountService.Infrastructure.Models;
using JetBrains.Annotations;
using AccountService.Api.ObjectStorage.Events.Published;

namespace AccountService.Api.Features.Account;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Models.Account, AccountViewModel>();
        CreateMap<CreateAccountCommand, Models.Account>();
        CreateMap<Models.Account, AccountOpened>();
    }
}
