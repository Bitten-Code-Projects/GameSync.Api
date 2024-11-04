using AutoMapper;
using GameSync.Application.Examples.UseCases.GetExampleById;
using GameSync.Domain.Examples.Interfaces;

namespace GameSync.Application.Examples.Mapping;

/// <summary>
/// AutoMapper profile for mapping Example-related objects.
/// </summary>
public class ExampleAutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleAutoMapperProfile"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor sets up the mapping configuration between IExample and GetExampleByIdResult.
    /// Note that for properties with the same name, explicit mapping is not required.
    /// </remarks>
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
