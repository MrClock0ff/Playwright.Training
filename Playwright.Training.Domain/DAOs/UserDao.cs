namespace Playwright.Training.Domain.DAOs;

public class UserDao
{
    public Guid Id { get; set; }
    
    public required string Username { get; set; }
    
    public required string Email { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDeleted { get; set; }
}