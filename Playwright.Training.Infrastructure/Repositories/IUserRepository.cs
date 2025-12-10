using Playwright.Training.Domain.DAOs;

namespace Playwright.Training.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<UserDao?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserDao>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    
    Task<UserDao> CreateUserAsync(UserDao inputDao, CancellationToken cancellationToken = default);
    
    Task<UserDao> UpdateUserAsync(UserDao inputDao, CancellationToken cancellationToken = default);
}