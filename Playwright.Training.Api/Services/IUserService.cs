using Microsoft.AspNetCore.JsonPatch;
using Playwright.Training.Domain.DTOs;

namespace Playwright.Training.Api.Services;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken = default);
    
    Task<UserDto> CreateUserAsync(CreateUserDto inputDto, CancellationToken cancellationToken = default);
    
    Task<UserDto?> DeleteUserWithIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<UserDto?> UpdateUserWithIdAsync(
        Guid userId,
        JsonPatchDocument<UserDto> patchDocument,
        CancellationToken cancellationToken = default);
}