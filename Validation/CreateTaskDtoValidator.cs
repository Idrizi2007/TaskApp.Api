using FluentValidation;
using TaskApp.Api.DTOS;

namespace TaskApp.Api.Validation
{
    public sealed class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);
        }
    }
}
