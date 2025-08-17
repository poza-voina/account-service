using Models = AccountService.Infrastructure.Models;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Infrastructure.Repositories.Interfaces;
using MediatR;
using AutoMapper;

namespace AccountService.Api.Features.Brokers;

public class CreateInboxConsumedCommandHandler(IRepository<Models.InboxConsumed> repository, IMapper mapper) : IRequestHandler<CreateInboxConsumedCommand, Unit>
{
    public async Task<Unit> Handle(CreateInboxConsumedCommand request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<Models.InboxConsumed>(request);
        
        await repository.AddAsync(model);

        return Unit.Value;
    }
}
