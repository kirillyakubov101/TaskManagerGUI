using FluentValidation;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Models.Validators;

public class SignUpDtoValidator :AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {

    }
}
