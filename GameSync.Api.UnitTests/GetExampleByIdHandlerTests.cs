using AutoMapper;
using FluentAssertions;
using FluentValidation;
using GameSync.Api.Application.Examples.Interfaces;
using GameSync.Api.Application.Examples.UseCases.GetExampleById;
using GameSync.Api.Domain.Examples.Entities;
using GameSync.Api.Domain.Examples.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace GameSync.Api.UnitTests;

public class GetExampleByIdHandlerTests
{
    private readonly IExampleRepository _exampleRepositoryMock = Substitute.For<IExampleRepository>();
    private readonly IValidator<GetExampleByIdQuery> _validatorMock = Substitute.For<IValidator<GetExampleByIdQuery>>();
    private readonly IMapper _mapperMock = Substitute.For<IMapper>();
    private readonly ILogger<GetExampleByIdHandler> _loggerMock = Substitute.For<ILogger<GetExampleByIdHandler>>();

    // All cases should be covered here

    [Fact]
    public async Task GetExampleByIdHandler_ReturnsExample_WhenExampleExists()
    {
        // Arrange
        const long exampleId = 5;
        GetExampleByIdQuery query = new GetExampleByIdQuery()
        {
            Id = exampleId,
        };

        _exampleRepositoryMock.GetExampleById(exampleId, default)
            .Returns(new Example()
            {
                Id = exampleId,
                Name = "Name",
                Surname = "Surname",
                Address = GetExampleAddress(),
            });

        _mapperMock.Map<GetExampleByIdResult>(Arg.Any<Example>()).Returns(new GetExampleByIdResult()
        {
            Id = exampleId,
            Name = "Name",
            Surname = "Surname",
            Address = GetExampleAddress(),
        });

        var handler = new GetExampleByIdHandler(_validatorMock, _exampleRepositoryMock, _mapperMock, _loggerMock);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        await _exampleRepositoryMock.Received(1).GetExampleById(exampleId);
        await _exampleRepositoryMock.DidNotReceive().GetExampleById(555);

        result.Should().NotBeNull();
        result.Id.Should().Be(exampleId);
        result.Name.Should().Be("Name");
        result.Surname.Should().Be("Surname");
        result.Address.Should().Be(GetExampleAddress());
    }

    private ExampleAddress GetExampleAddress()
    {
        return new ExampleAddress()
        {
            City = "city",
            HouseNumber = "123",
            Street = "street",
        };
    }
}