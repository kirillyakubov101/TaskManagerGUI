using Moq;
using TaskManagerGUI.Interfaces;
using Xunit;
using FluentAssertions;
using Moq.Protected;
using System.Net;
using SharedModels;
using System.Text;
using System.Text.Json;

namespace TaskManagerGUI.Services.Tests;

public class LoginEnterHanderTests
{
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _MessageHandlerMock;
    private readonly Mock<IAuthHandler> _AuthHandlerMock;
    private readonly LoginEnterHander _loginEnterHander;
    private readonly Mock<IErrorHandler> _ErrorHandlerMock;
    public LoginEnterHanderTests()
    {
        _MessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_MessageHandlerMock.Object);
        _AuthHandlerMock = new Mock<IAuthHandler>();
        _ErrorHandlerMock = new Mock<IErrorHandler>();
        _loginEnterHander = new LoginEnterHander(_httpClient, _AuthHandlerMock.Object, _ErrorHandlerMock.Object);
    }

    [Fact()]
    public async void GetAllUserTasks_WrongNoToken_ShouldFail()
    {
        // Arrange
        _AuthHandlerMock.Setup(x => x.IsAuthenticated()).Returns(false);
        _AuthHandlerMock.Setup(x => x.GetSessionToken()).Returns(string.Empty);
        // Act
        var result = await _loginEnterHander.GetAllUserTasks();

        // Assert
        
        _ErrorHandlerMock.Verify(x=>x.HandleError(It.IsAny<string>()),Times.Once);
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact()]
    public async void GetAllUserTasks_CorrectInput_ShouldSucceed()
    {
        // Arrange
        _AuthHandlerMock.Setup(x => x.IsAuthenticated()).Returns(true);
        _AuthHandlerMock.Setup(x => x.GetSessionToken()).Returns("TestToken");

        var userTasks = new List<UserTaskDto>
    {
        new UserTaskDto { Id = 1, Title = "Test Task", Description = "Description" },
        new UserTaskDto { Id = 2, Title = "Another Task", Description = "Another Description"}
    };

        var jsonResponse = JsonSerializer.Serialize(userTasks);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _MessageHandlerMock.Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
       .ReturnsAsync(responseMessage);

        // Act
        var result = await _loginEnterHander.GetAllUserTasks();

        // Assert
        _ErrorHandlerMock.Verify(x => x.HandleError(It.IsAny<string>()), Times.Never);
        result.Should().NotBeNull().And.NotBeEmpty();
        result.Should().HaveCount(2);
    }
}