// ComponentManagement.API/Dtos/UpdateComponentActivityRequest.cs
using Microsoft.AspNetCore.Http;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.API.Dtos
{
    public class UpdateComponentActivityRequest
    {
        // PDF atau dokumen lain (optional)
        public IFormFile? Documentation { get; set; }

        // Mekanik input list part tambahan / catatan
        public string? NeedSupport { get; set; }

        // Status baru (enum ComponentStatus)
        public ComponentStatus Status { get; set; }
    }
}
