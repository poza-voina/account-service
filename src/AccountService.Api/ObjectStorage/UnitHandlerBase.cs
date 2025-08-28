using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ViewModels;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AccountService.Api.ObjectStorage;

public abstract class UnitHandlerBase<TRequest, TResponse> 
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ApplicationContext _context;
    private readonly IUnitOfWork _unitOfWork;

    protected UnitHandlerBase(IServiceProvider serviceProvider)
    {
        _eventDispatcher = serviceProvider.GetRequiredService<IEventDispatcher>();
        _context = serviceProvider.GetRequiredService<ApplicationContext>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }

    protected abstract Task<TResponse> ExecuteTransactionBodyAsync(TRequest request, CancellationToken cancellationToken);

    protected virtual async Task<TResponse> ExecuteTransactionAsync(TRequest request, CancellationToken cancellationToken)
    {
        _context.IsTransaction = true;
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var result = await ExecuteTransactionBodyAsync(request, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _eventDispatcher.RollbackAll();
            throw;
        }
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (_context.IsTransaction)
        {
            return ExecuteTransactionBodyAsync(request, cancellationToken);
        }
        else
        {
            return ExecuteTransactionAsync(request, cancellationToken);
        }
    }
}
