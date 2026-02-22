using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class UpdateComponentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string NamaBrand { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public string NomerLaMbung { get; set; }
        public string Note { get; set; }
        public ComponentStatus Status { get; set; }

        // file opsional
        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
    }
}
