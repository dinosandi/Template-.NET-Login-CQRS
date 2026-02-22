using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Units.Commands;
using UnitEntity = ComponentManagement.Domain.Entities.Unit;
using MediatR;


namespace ComponentManagement.Application.Units.Handlers
{
    public class CreateUnitHandler : IRequestHandler<CreateUnitCommand, CreateUnitResponse>
    {
        private readonly IUnitRepository _unitRepository;

        public CreateUnitHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<CreateUnitResponse> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = new UnitEntity
            {
                Id = Guid.NewGuid(),
                NameUnit = request.NameUnit,
                // Description = request.Description,
                // Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return new CreateUnitResponse
            {
                Id = unit.Id,
            };
        }
    }
}
