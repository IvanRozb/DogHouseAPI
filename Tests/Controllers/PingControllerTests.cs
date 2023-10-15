using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Presentation.Controllers;

namespace Tests.Controllers;

public class PingControllerTests
{
    private readonly PingController _pingController;
    private readonly Mock<IConfiguration> _configurationMock;

    public PingControllerTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _pingController = new PingController(_configurationMock.Object);
    }

    [Fact]
    public void Ping_ValidConfiguration_ReturnsOk()
    {
        // Arrange
        _configurationMock.Setup(c => c.GetSection("ApiVersion").Value).Returns("Dogshouseservice.Version1");

        // Act
        var result = _pingController.Ping() as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Dogshouseservice.Version1", result.Value);
    }

    [Fact]
    public void Ping_ConfigurationNotSet_ReturnsBadRequestWithMessage()
    {
        // Arrange
        _configurationMock.Setup(c => c.GetSection("ApiVersion").Value).Returns((string)null);

        // Act
        var result = _pingController.Ping() as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("API version not configured.", result.Value);
    }

    [Fact]
    public void Ping_InvalidVersionFormat_ReturnsBadRequestWithMessage()
    {
        // Arrange
        _configurationMock.Setup(c => c.GetSection("ApiVersion").Value).Returns("InvalidVersion");

        // Act
        var result = _pingController.Ping() as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid API version format.", result.Value);
    }
}