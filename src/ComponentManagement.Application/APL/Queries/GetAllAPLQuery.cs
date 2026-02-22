using MediatR;

namespace ComponentManagement.Application.APLs.Queries
{
    public class GetAllAPLQuery : IRequest<PaginatedResult<APLDto>>
    {
        public string? NameBrand { get; set; }   // filter
        public int PageNumber { get; set; } = 1; // default page 1
        public int PageSize { get; set; } = 10;  // default 10
    }
    public class PaginatedResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

}
