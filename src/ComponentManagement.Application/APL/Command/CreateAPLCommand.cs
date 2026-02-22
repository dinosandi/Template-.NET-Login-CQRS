using MediatR;
using System;
using System.Collections.Generic;

namespace ComponentManagement.Application.APLs.Commands
{
    public class CreateAPLCommand : IRequest<CreateAPLResponse>
    {
        public string NameBrand { get; set; }
        
        public List<CreateAPLPartDto> Parts { get; set; } = new();
    }

    public class CreateAPLPartDto
    {
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateAPLResponse
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
