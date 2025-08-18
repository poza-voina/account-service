using AccountService.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using MediatR;
using Models = AccountService.Infrastructure.Models;

namespace AccountService.Api.Features.Brokers.CreateInboxConsumed;

public class CreateInboxConsumedCommandHandler(IRepository<Models.InboxConsumed> repository, IMapper mapper) : IRequestHandler<CreateInboxConsumedCommand, Unit>
{
    public async Task<Unit> Handle(CreateInboxConsumedCommand request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<Models.InboxConsumed>(request);
        
        await repository.AddAsync(model, cancellationToken);

        return Unit.Value;
    }
}
