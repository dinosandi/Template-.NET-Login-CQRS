using MediatR;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchPartComponentAPLCommand : IRequest<PatchPartComponentAPLResponse>
    {
        public Guid PartComponentId { get; set; }

        // 🔹 APL yang dipilih (wajib diisi, tapi hanya pilih existing)
        public string NameBrand { get; set; }

        // 🔹 Daftar APLPart yang sudah ada di APL (read-only di UI, tapi tetap dikirim untuk konteks)
        public List<APLPartDto> Parts { get; set; } = new();

        // 🔹 Optional: jika user menambahkan part baru custom (bukan dari APL)
        public List<ComponentCustomPartDto>? ComponentCustomParts { get; set; } = new();
    }

    // 🔹 Response
    public class PatchPartComponentAPLResponse
    {
        public Guid PartComponentId { get; set; }
        public APLDto UpdatedAPL { get; set; }
        public List<APLDto> AllAPLs { get; set; } = new();
    }

    // 🔹 DTO untuk APL utama
    public class APLDto
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<APLPartDto> Parts { get; set; } = new();
    }

    // 🔹 DTO untuk APLPart yang sudah ada
    public class APLPartDto
    {
        public Guid Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string? Priority { get; set; }
    }

    // 🔹 DTO untuk ComponentCustomPart (opsional, dibuat baru kalau diisi)
    public class ComponentCustomPartDto
    {
        public string NameBrand { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string? Priority { get; set; }
        public int Quantity { get; set; }
    }
}
