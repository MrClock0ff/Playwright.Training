using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Playwright.Training.Domain.DAOs;
using Playwright.Training.Domain.DTOs;
using Playwright.Training.Domain.Extensions;
using Playwright.Training.Infrastructure.Repositories;

namespace Playwright.Training.Api.Services;

public class UserService : IUserService
{
    private readonly IValidator<Guid> _guidValidator;
    private readonly IValidator<CreateUserDto> _createUseDtoValidator;
    private readonly IUserRepository _userRepository;

    public UserService(
        IValidator<Guid> guidValidator,
        IValidator<CreateUserDto> createUseDtoValidator,
        IUserRepository userRepository)
    {
        _guidValidator = guidValidator;
        _createUseDtoValidator = createUseDtoValidator;
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await _guidValidator.ValidateAndThrowAsync(id, cancellationToken);
        
        UserDao? resultDao = await _userRepository.GetUserByIdAsync(id, cancellationToken);
        UserDto? resultDto = resultDao?.ToUserDto();
        
        return resultDto;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken)
    {
        IEnumerable<UserDao> resultDaos = await _userRepository.GetAllUsersAsync(cancellationToken);
        IEnumerable<UserDto> resultDtos = resultDaos.Select(userDao => userDao.ToUserDto());

        return resultDtos;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto inputDto, CancellationToken cancellationToken)
    {
        await _createUseDtoValidator.ValidateAndThrowAsync(inputDto, cancellationToken);

        UserDao inputDao = new UserDao
        {
            Email = inputDto.Email,
            Username = inputDto.Username,
            FirstName = inputDto.FirstName,
            LastName = inputDto.LastName
        };
        UserDao resultDao = await _userRepository.CreateUserAsync(inputDao, cancellationToken);
        UserDto resultDto = resultDao.ToUserDto();

        return resultDto;
    }

    public async Task<UserDto?> DeleteUserWithIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _guidValidator.ValidateAndThrowAsync(userId, cancellationToken);

        UserDao? resultDao = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (resultDao == null)
        {
            return null;
        }

        resultDao.IsDeleted = true;

        resultDao = await _userRepository.UpdateUserAsync(resultDao, cancellationToken);
        UserDto resultDto = resultDao.ToUserDto();

        return resultDto;
    }

    public async Task<UserDto?> UpdateUserWithIdAsync(
        Guid userId,
        JsonPatchDocument<UserDto> patchDocument,
        CancellationToken cancellationToken)
    {
        await _guidValidator.ValidateAndThrowAsync(userId, cancellationToken);

        UserDao? resultDao = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (resultDao == null)
        {
            return null;
        }
        
        UserDto resultDto = resultDao.ToUserDto();
        patchDocument.ApplyTo(resultDto);

        resultDao.FirstName = resultDto.FirstName;
        resultDao.LastName = resultDao.LastName;
        
        await  _userRepository.UpdateUserAsync(resultDao, cancellationToken);
        
        return resultDto;
    }
}