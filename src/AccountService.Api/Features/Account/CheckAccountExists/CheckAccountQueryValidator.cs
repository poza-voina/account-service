using FluentValidation;

namespace AccountService.Api.Features.Account.CheckAccountExists;

public class CheckAccountQueryValidator : AbstractValidator<CheckAccountQuery>
{
    public CheckAccountQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
