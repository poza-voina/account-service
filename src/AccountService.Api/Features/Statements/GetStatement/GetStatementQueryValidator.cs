using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Statements.GetStatement;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
{
    public GetStatementQueryValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .WithMessage("Идентификатор клиента не может быть пустым");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Идентификатор счет не может быть пустым");

        RuleFor(x => x)
            .Must(x => x.StartDateTime is null || x.EndDateTime is null || x.StartDateTime <= x.EndDateTime)
            .WithMessage("Диапазон выписки может быть (-inf; +inf) (-inf; date] [date; +inf) [date; date]");
    }
}
