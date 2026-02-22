using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.APLs.Queries
{
    public class GetAllAPLQueryHandler : IRequestHandler<GetAllAPLQuery, PaginatedResult<APLDto>>
    {
        private readonly IAPLRepository _aplRepository;

        public GetAllAPLQueryHandler(IAPLRepository aplRepository)
        {
            _aplRepository = aplRepository;
        }

        public async Task<PaginatedResult<APLDto>> Handle(GetAllAPLQuery request, CancellationToken cancellationToken)
        {
            var query = await _aplRepository.QueryAllAsync();

            // filter NameBrand
            if (!string.IsNullOrWhiteSpace(request.NameBrand))
            {
                query = query.Where(a => EF.Functions.ILike(a.NameBrand, $"%{request.NameBrand}%"));
            }


            var totalCount = query.Count();

            var items = query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new APLDto
                {
                    Id = a.Id,
                    NameBrand = a.NameBrand,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Parts = a.Parts.Select(p => new APLPartDto
                    {
                        Id = p.Id,
                        PartNumber = p.PartNumber,
                        Description = p.Description,
                        Priority = p.Priority,
                        Quantity = p.Quantity
                    }).ToList()
                })
                .ToList();


            return new PaginatedResult<APLDto>
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = items
            };
        }
    }
}
