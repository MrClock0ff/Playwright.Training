using FluentValidation;
using Playwright.Training.Api.Settings;

namespace Playwright.Training.Api.Validators;

public class KeycloakSettingsValidator: AbstractValidator<KeycloakSettings>
{
    public KeycloakSettingsValidator()
    {
        RuleFor(x => x.Authority)
            .NotEmpty()
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.Authority)} is required and cannot be empty")
            .NotEqual("parameter")
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.Authority)} has wrong value: parameter");
        
        RuleFor(x => x.ValidIssuer)
            .NotEmpty()
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.ValidIssuer)} is required and cannot be empty")
            .NotEqual("parameter")
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.ValidIssuer)} has wrong value: parameter");
        
        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.Audience)} is required and cannot be empty")
            .NotEqual("parameter")
            .WithMessage($"{nameof(KeycloakSettingsValidator)}: {nameof(KeycloakSettings.Audience)} has wrong value: parameter");
    }
}