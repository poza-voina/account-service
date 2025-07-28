using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Account.CheckAccountExists;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CheckAccountQueryValidator : AbstractValidator<CheckAccountQuery>
{
    public CheckAccountQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
