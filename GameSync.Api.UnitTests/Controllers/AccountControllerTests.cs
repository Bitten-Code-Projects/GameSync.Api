using Xunit;
using Shouldly;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GameSync.Api.Controllers;
using GameSync.Application.Account.Dtos;
using GameSync.Infrastructure.Context.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace GameSync.Api.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<ILogger<AccountController>> _loggerMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        _loggerMock = new Mock<ILogger<AccountController>>();

        _controller = new AccountController(_userManagerMock.Object, _loggerMock.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
            {
                Connection = { RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1") }
            }
        };
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserCreatedSuccessfully()
    {
        // Arrange
        var dto = new RegisterRequestDto
        {
            Login = "testuser",
            Email = "test@example.com",
            Password = "Test123!"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var dto = new RegisterRequestDto
        {
            Login = "testuser",
            Email = "test@example.com",
            Password = "weak"
        };

        var errors = new List<IdentityError>
        {
            new IdentityError { Description = "Password is too weak." }
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        // Act
        var result = await _controller.Register(dto);

        // Assert
        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;
        var errorList = badRequest?.Value as IEnumerable<string>;
        errorList?.ShouldContain("Password is too weak.");
    }
}