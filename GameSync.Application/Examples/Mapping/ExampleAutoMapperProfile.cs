using AutoMapper;
using GameSync.Application.Examples.UseCases.GetExampleById;
using GameSync.Domain.Examples.Interfaces;

namespace GameSync.Application.Examples.Mapping;

public class ExampleAutoMapperProfile : Profile
{
    public ExampleAutoMapperProfile()
    {
        // Just an example, mapping to the properties with same name doesn't require explicit mapping. Simple CreateMap is enough
        CreateMap<IExample, GetExampleByIdResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}
