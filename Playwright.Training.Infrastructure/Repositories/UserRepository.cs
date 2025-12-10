using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Playwright.Training.Domain.DAOs;

namespace Playwright.Training.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TrainingDatabaseContext _dbContext;

    public UserRepository(TrainingDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserDao>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        List<UserDao> result = await _dbContext.Users
            .Where(u => !u.IsDeleted)
            .ToListAsync(cancellationToken);
        return result;
    }

    public async Task<UserDao> CreateUserAsync(UserDao inputDao, CancellationToken cancellationToken)
    {
        EntityEntry<UserDao> result = await _dbContext.Users.AddAsync(inputDao, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<UserDao> UpdateUserAsync(UserDao inputDao, CancellationToken cancellationToken)
    {
        EntityEntry<UserDao> result = _dbContext.Users.Update(inputDao);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<UserDao?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        UserDao? result = await _dbContext.Users
            .Where(u => !u.IsDeleted)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return result;
    }
}