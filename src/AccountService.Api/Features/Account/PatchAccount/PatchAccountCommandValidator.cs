using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Account.PatchAccount;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
{
    public PatchAccountCommandValidator()
    {
        RuleFor(x => x.ClosingDate)
            .GreaterThanOrEqualTo(x => DateTime.Today);
    }
}
