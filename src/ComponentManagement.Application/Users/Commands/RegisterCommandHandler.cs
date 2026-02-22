using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;

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
            // ===== Validasi =====
            
            // 1. Username
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new InvalidOperationException("Username tidak boleh kosong");
            if (request.Username.Length > 30)
                throw new InvalidOperationException("Username maksimal 30 karakter");

            // 2. NoHp
            if (string.IsNullOrWhiteSpace(request.NoHp))
                throw new InvalidOperationException("NoHp tidak boleh kosong");
            if (!Regex.IsMatch(request.NoHp, @"^\d{12,13}$"))
                throw new InvalidOperationException("NoHp harus 12-13 digit dan hanya angka");

            // 3. Email
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new InvalidOperationException("Email tidak boleh kosong");
            if (request.Email.Length < 8)
                throw new InvalidOperationException("Email minimal 8 karakter");
            if (!Regex.IsMatch(request.Email, @"^[\w\.-]+@[\w\.-]+\.\w{2,}$"))
                throw new InvalidOperationException("Email tidak valid");

            // 4. Cek username sudah ada
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username sudah digunakan");

            // ===== Proses Registrasi =====
            
            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Buat entity user baru
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                NoHp = request.NoHp,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            // Simpan ke repository
            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            // ===== Response =====
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
