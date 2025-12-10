using Playwright.Training.Domain.DAOs;
using Playwright.Training.Domain.DTOs;

namespace Playwright.Training.Domain.Extensions;

public static class UserDaoExtensions
{
    public static UserDto ToUserDto(this UserDao dao)
    {
        UserDto dto = new UserDto(
            dao.Id,
            dao.Username,
            dao.Email,
            dao.FirstName,
            dao.LastName);
        
        return dto;
    }
}