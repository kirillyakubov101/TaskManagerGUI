using Moq;
using TaskManagerGUI.Interfaces;
using Xunit;
using FluentAssertions;
using System.Net;
using Moq.Protected;

namespace TaskManagerGUI.Services.Tests;

public class DeleteTaskHandlerTests
{
    private readonly Mock<IAuthHandler> _authHandlerMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly DeleteTaskHandler _deleteTaskHandler;
    private readonly Mock<IErrorHandler> _errorHandlerMock;

    public DeleteTaskHandlerTests()
    {
        _authHandlerMock = new Mock<IAuthHandler>();
        _httpMessageHandlerMock= new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _errorHandlerMock = new Mock<IErrorHandler>();
        _deleteTaskHandler = new DeleteTaskHandler(_authHandlerMock.Object,_httpClient, _errorHandlerMock.Object);
    }

    [Fact()]
    public async void DeleteTask_NoTokenStored_ShouldFail()
    {
        int taskId = 1;
        // Arrange
        _authHandlerMock.Setup(a => a.GetSessionToken()).Returns(string.Empty);


        // Act
        var result = await _deleteTaskHandler.Delete(taskId);

        // Assert
        result.Should().BeFalse();
        _errorHandlerMock.Verify(x=> x.HandleError(It.IsAny<string>()), Times.Once);

    }

    [Fact()]
    public async void DeleteTask_CorrectInput_ShouldSucceed()
    {
        // Arrange
        int taskId = 1;
        _authHandlerMock.Setup(a => a.IsAuthenticated()).Returns(true);
        _authHandlerMock.Setup(a => a.GetSessionToken()).Returns("Token");

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NoContent
        };

       _httpMessageHandlerMock
      .Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
      .ReturnsAsync(responseMessage);


        // Act
        var result = await _deleteTaskHandler.Delete(taskId);

        // Assert
        result.Should().BeFalse();

    }
}
