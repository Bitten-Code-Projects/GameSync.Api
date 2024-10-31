using FluentAssertions;
using GameSync.Api.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using GameSync.Application.Examples.Interfaces;
using GameSync.Domain.Examples.Entities;
using GameSync.Domain.Examples.ValueObjects;
using GameSync.Application.Examples.UseCases.GetExampleById;
using GameSync.Api.Shared.Middleware.Models;

namespace GameSync.Api.IntegrationTests;

public class ExampleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    private readonly IExampleRepository _exampleRepositoryMock = Substitute.For<IExampleRepository>();

    public ExampleControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetExample_ReturnsExample_WhenExampleExists()
    {
        // Arrange
        const long Id = 1;
        _exampleRepositoryMock.GetExampleById(Id, Arg.Any<CancellationToken>()).Returns(
            new Example()
            {
                Id = Id,
                Name = "test name",
                Surname = "test surname",
                Address = new ExampleAddress()
                {
                    City = "test city",
                    HouseNumber = "test house number",
                    Street = "test street",
                },
            });

        var client = ClientFactory.GetHttpClientWithMocks(_factory, new Dictionary<Type, object>()
        {
            { typeof(IExampleRepository), _exampleRepositoryMock },
        });

        // Act
        var response = await client.GetAsync($"/api/example/{Id}");

        var result = await JsonUtilities<GetExampleByIdResult>.DeserializeResponse(response);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        result.Should().NotBeNull();
        result!.Id.Should().Be(Id);
        result.Name.Should().Be("test name");
        result.Surname.Should().Be("test surname");
        result.Address.Should().Be(new ExampleAddress()
        {
            City = "test city",
            HouseNumber = "test house number",
            Street = "test street",
        });
    }

    [Fact]
    public async Task GetExample_NotReturnsExample_WhenExampleDoesNotExists()
    {
        // Arrange
        const long Id = 1;
        _exampleRepositoryMock.GetExampleById(Id, Arg.Any<CancellationToken>()).ReturnsNull();

        var client = ClientFactory.GetHttpClientWithMocks(_factory, new Dictionary<Type, object>()
        {
            { typeof(IExampleRepository), _exampleRepositoryMock },
        });

        // Act
        var response = await client.GetAsync($"/api/example/{Id}");

        var result = await JsonUtilities<ErrorDetails>.DeserializeResponse(response);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json");

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        result.Message.Should().Be($"Example with id {Id} was not found");
        result.ValidationErrors.Should().BeEmpty();
    }
}
