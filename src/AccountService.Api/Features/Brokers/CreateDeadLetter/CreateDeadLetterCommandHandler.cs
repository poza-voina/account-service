using AccountService.Infrastructure.Repositories.Interfaces;
using Models = AccountService.Infrastructure.Models;
using MediatR;
using AutoMapper;

namespace AccountService.Api.Features.Brokers.CreateDeadLetter;

public class CreateDeadLetterCommandHandler(IRepository<Models.InboxDeadLetter> repository, IMapper mapper) : IRequestHandler<CreateDeadLetterCommand, Unit>
{
    public async Task<Unit> Handle(CreateDeadLetterCommand request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<Models.InboxDeadLetter>(request);

        await repository.AddAsync(model);

        return Unit.Value;
    }
}
