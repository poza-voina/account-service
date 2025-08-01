using AccountService.Api.ObjectStorage.Interfaces;
using FluentValidation;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransferTransactionCommandValidator : AbstractValidator<TransferTransactionCommand>
{
    public TransferTransactionCommandValidator(ICurrencyHelper currencyHelper)
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty()
            .WithMessage("������������� ����� �� ����� ���� ������");

        RuleFor(x => x.CounterpartyBankAccountId)
            .NotEmpty()
            .WithMessage("������������� ����� ����������� �� ����� ���� ������");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("���������� ����� ������ ���� ������ 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currencyHelper.IsValid)
            .WithMessage("������ ������ ���� ������� � ������� ISO 4217");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("�������� �� ������ ���� ������");
    }
}
