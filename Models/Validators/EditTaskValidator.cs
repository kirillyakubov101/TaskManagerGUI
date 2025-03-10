using FluentValidation;
using SharedModels;

namespace TaskManagerGUI.Models.Validators;

public class EditTaskValidator : AbstractValidator<UserTaskEditDto>
{
    public EditTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(20)
          .WithMessage("Title should not be empty, less than 3 characters or greater than 20");

        RuleFor(x => x.Description).NotEmpty().MinimumLength(5).MaximumLength(100)
            .WithMessage("Description should not be empty, less than 5 characters or greater than 100");
    }
}
