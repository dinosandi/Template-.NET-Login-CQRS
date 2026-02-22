using MediatR;

namespace ComponentManagement.Application.APLs.Commands
{
    public class UpdateAPLCommand : IRequest<UpdateAPLResponse>
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public List<UpdateAPLPartDto>? Parts { get; set; } // Bagian yang diupdate atau dihapus

    }

    public class UpdateAPLResponse
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<UpdateAPLPartResponse>? Parts { get; set; }
    }
    public class UpdateAPLPartDto
    {
        public Guid Id { get; set; }
        public string? PartNumber { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } // kalau true → hapus
    }
    public class UpdateAPLPartResponse
    {
        public Guid Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }


}
