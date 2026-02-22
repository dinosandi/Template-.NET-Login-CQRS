using ComponentManagement.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ComponentManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
        Task<User> GetByIdAsync(Guid userId);
    }
}
