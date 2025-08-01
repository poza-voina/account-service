using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Account.RemoveAccount;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RemoveAccountCommandValidator : AbstractValidator<RemoveAccountCommand>
{
    public RemoveAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Идентификатор счета не может быть пустым");
    }
}
