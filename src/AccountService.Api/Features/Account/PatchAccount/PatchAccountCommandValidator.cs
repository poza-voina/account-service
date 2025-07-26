using FluentValidation;

namespace AccountService.Api.Features.Account.PatchAccount;

public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
{
    public PatchAccountCommandValidator()
    {
        RuleFor(x => x.ClosingDate)
            .GreaterThanOrEqualTo(x => DateTime.Today);
    }
}
