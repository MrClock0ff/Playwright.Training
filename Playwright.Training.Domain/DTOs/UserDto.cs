namespace Playwright.Training.Domain.DTOs;

public record UserDto(Guid Id, string Username, string Email, string? FirstName, string? LastName);