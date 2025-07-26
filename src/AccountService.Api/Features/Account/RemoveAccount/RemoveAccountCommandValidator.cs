using FluentValidation;

namespace AccountService.Api.Features.Account.RemoveAccount;

public class RemoveAccountCommandValidator : AbstractValidator<RemoveAccountCommand>
{
    public RemoveAccountCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
