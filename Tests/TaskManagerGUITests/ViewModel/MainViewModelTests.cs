using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Validators;
using Xunit;

namespace TaskManagerGUI.ViewModel.Tests;

public class MainViewModelTests
{

    // TestMethod_Scenario_ExpectedResult
    [Fact]
    public void SignInAsync_IncorrectSignDto_ShouldFail()
    {
        // Arrange
        SignInDtoValidator validationRules = new SignInDtoValidator();
        SignInDto validationDto = new SignInDto()
        {
            Email = "test@@testcom",
            Password = "pass",
        };

        // Act
        var validationResult = validationRules.TestValidate(validationDto);

        // Assert
        validationResult.ShouldHaveAnyValidationError();
        validationResult.ShouldHaveValidationErrorFor(memberAccessor => memberAccessor.Email);
        validationResult.ShouldHaveValidationErrorFor(memberAccessor => memberAccessor.Password);

    }

    [Fact]
    public void SignInAsync_CorrectSignDto_ShouldSucceed()
    {
        // Arrange
        var validationRules = new SignInDtoValidator();
        var validationDto = new SignInDto()
        {
            Email = "test@test.com",
            Password = "Password1!",
        };

        // Act
        var validationResult = validationRules.TestValidate(validationDto);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}