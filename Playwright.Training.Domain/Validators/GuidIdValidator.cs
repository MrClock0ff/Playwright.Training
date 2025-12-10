using FluentValidation;

namespace Playwright.Training.Domain.Validators;

public class GuidIdValidator : AbstractValidator<Guid>
{
    public GuidIdValidator()
    {
        RuleFor(guid => guid)
            .NotEmpty()
            .WithMessage("Guid Id cannot be empty");
    }
}