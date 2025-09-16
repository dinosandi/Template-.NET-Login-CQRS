// Infrastructure/Services/UserService.cs
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}