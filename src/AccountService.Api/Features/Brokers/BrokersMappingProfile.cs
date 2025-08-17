using AccountService.Api.Features.Brokers.CreateDeadLetter;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Infrastructure.Models;
using AutoMapper;
using JetBrains.Annotations;

namespace AccountService.Api.Features.Brokers;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class BrokersMappingProfile : Profile
{
    public BrokersMappingProfile()
    {
        CreateMap<CreateDeadLetterCommand, InboxDeadLetter>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.FailedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CreateInboxConsumedCommand, InboxConsumed>();
    }
}
