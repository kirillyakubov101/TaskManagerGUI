using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Enums;
using Xunit;

namespace TaskManagerGUI.Services.Tests;

public class SignInHandlerTests
{
    private readonly Mock<IAuthHandler> _authHandlerMock;
    private readonly Mock<INavigationService> _navigationMock;
    private readonly Mock<IErrorHandler> _errorHandlerMock;

    public SignInHandlerTests()
    {
        _authHandlerMock = new Mock<IAuthHandler>();
        _navigationMock = new Mock<INavigationService>();
        _errorHandlerMock = new Mock<IErrorHandler>();
    }

    [Fact]
    public async Task SignIn_ValidCredentials_ShouldStoreTokenAndNavigate()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Email = "test@test.com",
            Password = "Password1!"
        };

        var fakeResponse = new SignInTokenResponse { AccessToken = "mock_token" };
        var jsonResponse = JsonSerializer.Serialize(fakeResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

       var httpClient = new HttpClient(httpMessageHandlerMock.Object);
       var signInHandler = new SignInHandler(httpClient, _authHandlerMock.Object, _navigationMock.Object, _errorHandlerMock.Object);

        _authHandlerMock.Setup(a => a.GetSessionToken()).Returns("mock_token");

        // Act
        await signInHandler.SignIn(signInDto);

        // Assert
        _authHandlerMock.Verify(a => a.RegisterSessionToken("mock_token"), Times.Once);
        var token = _authHandlerMock.Object.GetSessionToken();
        token.Should().Be("mock_token");

        _authHandlerMock.Verify(a => a.GetSessionToken(), Times.Once);
        _navigationMock.Verify(n => n.OpenWindow(WindowType.Dashboard), Times.Once);
        _navigationMock.Verify(n => n.CloseWindow(WindowType.LoginWindow), Times.Once);
    }
}