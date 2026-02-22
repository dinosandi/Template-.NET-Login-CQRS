using MediatR;
using ComponentManagement.Application.Users.Queries;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentManagement.Application.Users.Handlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PaginatedResult<UserDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllUserQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsQueryable();

            // Filter nama
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                query = query.Where(u => u.Username.Contains(request.Username));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    NoHp = u.NoHp,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResult<UserDto>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
