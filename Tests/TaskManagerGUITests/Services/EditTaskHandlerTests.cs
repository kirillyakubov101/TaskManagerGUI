using FluentAssertions;
using Moq;
using Moq.Protected;
using SharedModels;
using System.Net;
using TaskManagerGUI.Interfaces;
using Xunit;

namespace TaskManagerGUI.Services.Tests;

public class EditTaskHandlerTests
{
    private readonly Mock<IAuthHandler> _authHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly EditTaskHandler _editTaskHandler;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<IErrorHandler> _errorHandlerMock;

    public EditTaskHandlerTests()
    {
        _authHandlerMock = new Mock<IAuthHandler>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _errorHandlerMock = new Mock<IErrorHandler>();
        _editTaskHandler = new EditTaskHandler(_authHandlerMock.Object, _httpClient, _errorHandlerMock.Object);
    }

    [Fact()]
    public async void EditTask_NoToken_ShouldFail()
    {
        // Arrange
        _authHandlerMock.Setup(x => x.IsAuthenticated()).Returns(false);
        int taskId = 1;
        UserTaskEditDto userTaskDto = new UserTaskEditDto()
        {
            Description = "description",
            DueDate = DateTime.Now,
            Priority = 0,
            Status = 0
        };


        // Act
        bool result = await _editTaskHandler.EditTask(taskId, userTaskDto);

        // Assert
        result.Should().BeFalse();
        _errorHandlerMock.Verify(x => x.HandleError(It.IsAny<string>()), Times.Once());
    }


    [Fact()]
    public async void EditTask_CorrectInput_ShouldSucceed()
    {
        // Arrange
        _authHandlerMock.Setup(x => x.IsAuthenticated()).Returns(true);
        int taskId = 1;
        UserTaskEditDto userTaskDto = new UserTaskEditDto()
        {
            Description = "description",
            DueDate = DateTime.Now,
            Priority = 0,
            Status = 0
        };

        HttpResponseMessage responseMessage = new HttpResponseMessage
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
        bool result = await _editTaskHandler.EditTask(taskId, userTaskDto);


        // Assert
        result.Should().BeTrue();
        _errorHandlerMock.Verify(x => x.HandleError(It.IsAny<string>()), Times.Never());
    }

    [Fact()]
    public async void EditTask_InternalError_ShouldFail()
    {
        // Arrange
        _authHandlerMock.Setup(x => x.IsAuthenticated()).Returns(true);
        int taskId = 1;
        UserTaskEditDto userTaskDto = new UserTaskEditDto()
        {
            Description = "description",
            DueDate = DateTime.Now,
            Priority = 0,
            Status = 0
        };

        HttpResponseMessage responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadGateway
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
        bool result = await _editTaskHandler.EditTask(taskId, userTaskDto);


        // Assert
        result.Should().BeFalse();
        _errorHandlerMock.Verify(x => x.HandleError(It.IsAny<string>()), Times.Once());
    }

}