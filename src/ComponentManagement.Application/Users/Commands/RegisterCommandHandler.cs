using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using MediatR;


namespace ComponentManagement.Application.Users.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // cek username sudah ada
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            // hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // langsung pakai enum role dari request
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return new RegisterResponse
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Role = newUser.Role,
                CreatedAt = newUser.CreatedAt
            };
        }
    }
}
