using api_bus_tickets.DTOs;

namespace api_bus_tickets.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto?> AuthenticateUserAsync(LoginDto loginDto);
        Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword);
    }
} 