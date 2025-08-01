using AccountService.Api.ObjectStorage;
using AccountService.Api.ObjectStorage.Interfaces;
using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Transactions.RegisterTransaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RegisterTransactionCommandValidator : AbstractValidator<RegisterTransactionCommand>
{
    public RegisterTransactionCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty()
            .WithMessage("Идентификатор счета не может быть пустым");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Количество денег должно быть больше 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid)
            .WithMessage("Валюта должна быть введена в формате ISO 4217");
        
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Несуществующий тип транзакции");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Описание не может быть пустым");
    }
}