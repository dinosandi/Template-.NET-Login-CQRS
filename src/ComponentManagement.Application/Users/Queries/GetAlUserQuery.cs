using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Users.Queries
{
    public class GetAllUserQuery : IRequest<PaginatedResult<UserDto>>
    {
        public string? Username { get; set; }      // untuk filter nama (optional)
        public int PageNumber { get; set; } = 1;     // default halaman 1
        public int PageSize { get; set; } = 10;      // default 10 data per halaman
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string NoHp { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // wrapper untuk hasil paginasi
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
