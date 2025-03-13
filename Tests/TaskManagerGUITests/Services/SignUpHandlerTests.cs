﻿using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows;
using TaskManagerGUI.Interfaces;
using Xunit;

namespace TaskManagerGUI.Services.Tests;

public class SignUpHandlerTests
{
    private readonly SignUpHandler _signUpHandler;
    private readonly Mock<INavigationService> _navigationServiceMock;
    private readonly Mock<IErrorHandler> _errorHandlerMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    public SignUpHandlerTests()
    {
        _navigationServiceMock = new Mock<INavigationService>();
        _errorHandlerMock = new Mock<IErrorHandler>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _signUpHandler = new SignUpHandler(_navigationServiceMock.Object, _httpClient, _errorHandlerMock.Object);
    }

    [Fact()]
    public async void IsEmailAvailable_AvailableEmail_ShouldSucceed()
    {
        // Arrange
        var email = "available@email.com";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _signUpHandler.IsEmailAvailable(email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact()]
    public async void IsEmailAvailable_NonAvailableEmail_ShouldFail()
    {
        // Arrange
        var email = "Nonavailable@email.com";
        var errorMessage = "Email is already in use.";
        var SignUpEmailErrorResponse = new SignUpEmailErrorResponse() { message = errorMessage };
        var jsonResponse = JsonSerializer.Serialize(SignUpEmailErrorResponse);

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _signUpHandler.IsEmailAvailable(email);

        // Assert
        result.Should().BeFalse();
        _errorHandlerMock.Verify(e => e.HandleError(errorMessage, MessageBoxImage.Error), Times.Once);
    }

    [Fact()]
    public async void IsEmailAvailable_NonAvailableEmailNoMessage_ShouldFail()
    {
        // Arrange
        var email = "Nonavailable@email.com";
        var errorMessage = "Available email validation failed";
        var jsonResponse = "{}";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _signUpHandler.IsEmailAvailable(email);

        // Assert
        result.Should().BeFalse();
        _errorHandlerMock.Verify(e => e.HandleError(errorMessage, MessageBoxImage.Error), Times.Once);
    }

    [Fact()]
    public async void CreateUser_CorrectUserDtoPassed_ShouldSucceed()
    {
        //
    }
}