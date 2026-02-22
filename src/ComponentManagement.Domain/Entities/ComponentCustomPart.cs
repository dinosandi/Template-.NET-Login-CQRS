using System;

namespace ComponentManagement.Domain.Entities
{

    public class ComponentCustomPart
{
    public Guid Id { get; set; }
    public Guid PartComponentId { get; set; }

    public string NameBrand { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public PartComponent PartComponent { get; set; } = null!;
}

}
