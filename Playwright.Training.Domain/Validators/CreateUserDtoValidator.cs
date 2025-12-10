using FluentValidation;
using Playwright.Training.Domain.DTOs;

namespace Playwright.Training.Domain.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .NotNull()
            .WithMessage("Username is required")
            .Matches(@"^[a-zA-Z0-9_\-\.]+$");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .NotNull()
            .WithMessage("Email is required")
            .EmailAddress();

        RuleFor(user => user.FirstName)
            .Matches(@"^[a-zA-Z\-\.]+$");
        
        RuleFor(user => user.LastName)
            .Matches(@"^[a-zA-Z\-\.]+$");
    }
}