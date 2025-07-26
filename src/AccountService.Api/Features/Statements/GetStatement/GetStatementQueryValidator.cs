using FluentValidation;

namespace AccountService.Api.Features.Statement.GetStatement;

public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
{
    public GetStatementQueryValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty();

        RuleFor(x => x.AccountId)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x => x.StartDateTime is null || x.EndDateTime is null || x.StartDateTime <= x.EndDateTime);
    }
}
