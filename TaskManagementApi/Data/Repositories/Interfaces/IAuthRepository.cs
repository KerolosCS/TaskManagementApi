using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        string CreateToken(User user);
    }
}
