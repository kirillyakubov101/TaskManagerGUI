using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Validators;
using TaskManagerGUI.Services;
using Xunit;


namespace TaskManagerGUI.ViewModel.Tests;

public class SignUpViewModelTests
{
    private readonly SignUpHandler signUpHandler;
    public SignUpViewModelTests()
    {
        //signUpHandler = new SignUpHandler()
    }

    [Theory()]
    [InlineData("Password1!","Password1!",false,true)]
    [InlineData("Password1!", "Password2!", false, false)]
    [InlineData("Password1!", "Password1!", true, false)]
    public void CanExecuteSignUpTest_ForValidAndInvalidPasswordMatch_ShouldReturnTrueAndFalse(string password,string confirmPassword, bool isMidSignUp, bool expectedResult)
    {
        Mock<IMessageService> messageservice = new Mock<IMessageService>();
        // arrange
        SignUpViewModel signUpViewModel = new SignUpViewModel(null, null, messageservice.Object);
        signUpViewModel.Password = password;
        signUpViewModel.ConfirmPassword = confirmPassword;
        signUpViewModel.MidSignUpProcess = isMidSignUp;


        // act
        var result = signUpViewModel.SignUpCommand.CanExecute(null);


        // assert
        result.Should().Be(expectedResult);
        if (!expectedResult)
        {
            messageservice.Verify(x => x.ShowMessage("Password and confirm password don't match!"), Times.Once());
        }
    }

    [Fact()]
    public void SignUpValidation_CorrectSignUpDto_ShouldSucceed()
    {
        // arrange
        var signupDto = new SignUpDto
        {
            Nickname ="TestNick",
            Email = "Test@test.com",
            Password = "Password1!",
        };

        SignUpDtoValidator validator = new SignUpDtoValidator();

        // act

        var result = validator.TestValidate(signupDto);

        // assert

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void SignUpValidation_WrongSignUpDto_ShouldFail()
    {
        // arrange
        var signupDto = new SignUpDto
        {
            Nickname = "Test",
            Email = "Test@@test..",
            Password = "Password12",
        };

        SignUpDtoValidator validator = new SignUpDtoValidator();

        // act

        var result = validator.TestValidate(signupDto);

        // assert

        result.ShouldHaveValidationErrorFor(x => x.Nickname);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}