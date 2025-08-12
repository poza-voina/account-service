using AccountService.Api.Features.Transactions.ApplyTransactionPair;
using AccountService.Api.Features.Transactions.RegisterTransaction;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AutoMapper;
using MediatR;
using System.Data;

namespace AccountService.Api.Features.Transactions.TransferTransaction;

public class TransferTransactionCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) : IRequestHandler<TransferTransactionCommand, TransferTransactionViewModel>
{
    public async Task<TransferTransactionViewModel> Handle(TransferTransactionCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var credit = await mediator.Send(BuildCreditTransaction(request), cancellationToken);
            var debit = await mediator.Send(BuildDebitTransaction(request), cancellationToken);

            var applyTransactionsCommand = new ApplyTransactionPairCommand
            {
                CreditTransaction = new TransactionInfo
                {
                    TransactionId = credit.Id,
                    AccountVersion = request.BankAccountVersion
                },
                DebitTransaction = new TransactionInfo
                {
                    TransactionId = debit.Id,
                    AccountVersion = request.CounterpartyBankAccountVersion
                }
            };

            await mediator.Send(applyTransactionsCommand, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            return new TransferTransactionViewModel { Credit = credit, Debit = debit };
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private RegisterTransactionCommand BuildCreditTransaction(TransferTransactionCommand request)
    {
        var registerCommand = mapper.Map<RegisterTransactionCommand>(request);
        registerCommand.Type = TransactionType.Credit;

        return registerCommand;
    }

    private RegisterTransactionCommand BuildDebitTransaction(TransferTransactionCommand request)
    {
        var registerCommand = mapper.Map<RegisterTransactionCommand>(request);
        registerCommand.BankAccountId = request.CounterpartyBankAccountId;
        registerCommand.CounterpartyBankAccountId = request.CounterpartyBankAccountId;
        registerCommand.Type = TransactionType.Debit;

        return registerCommand;
    }
}
