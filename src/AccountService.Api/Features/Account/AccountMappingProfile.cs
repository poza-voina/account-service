using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.ViewModels;
using AutoMapper;

namespace AccountService.Api.Features.Account;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Domains.Account, AccountViewModel>();
        CreateMap<CreateAccountCommand, Domains.Account>();
    }
}
