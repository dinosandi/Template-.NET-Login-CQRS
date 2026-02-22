using System;
using MediatR;

namespace ComponentManagement.Application.Components.Commands
{
    public class InstallComponentCommand : IRequest<Guid>
    {

        public Guid PartComponentId { get; set; }

        public double TotalLifetimeHm { get; set; }

        // WAKTU LOKAL USER (JANGAN UTC)
        public DateTimeOffset InstalledAt { get; set; }


    }

}
