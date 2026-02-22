using System;

namespace ComponentManagement.Domain.Entities
{
    public class PartComponentAPL
{
    public Guid Id { get; set; }
    public Guid PartComponentId { get; set; }
    public Guid? APLId { get; set; }

    public PartComponent PartComponent { get; set; }
    public APL? APL { get; set; }
}
}
