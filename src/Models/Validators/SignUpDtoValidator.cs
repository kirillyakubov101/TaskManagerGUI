using FluentValidation;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Models.Validators;

public class SignUpDtoValidator :AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(x => x.Nickname)
          .NotEmpty().WithMessage("Nickname is required.")
          .MinimumLength(6).WithMessage("Nickname must be at least 6 characters long.");

        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email is required.")
           .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("Password must contain at least one special character.");

      
    }
}
