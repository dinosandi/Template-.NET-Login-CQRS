using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Units.Commands;

namespace ComponentManagement.Application.Units.Handlers
{
    public class UpdateUnitHandler : IRequestHandler<UpdateUnitCommand, UpdateUnitResponse>
    {
        private readonly IUnitRepository _unitRepository;

        public UpdateUnitHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UpdateUnitResponse> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _unitRepository.GetByIdAsync(request.Id);
            if (unit == null)
            {
                throw new KeyNotFoundException($"Unit dengan ID {request.Id} tidak ditemukan.");
            }

            unit.NameUnit = request.NameUnit;
            // unit.Description = request.Description;
            // unit.Status = request.Status;
            unit.UpdatedAt = DateTime.UtcNow;

            _unitRepository.Update(unit);
            await _unitRepository.SaveChangesAsync();

            return new UpdateUnitResponse
            {
                Id = unit.Id,
                NameUnit = unit.NameUnit,
                // Description = unit.Description,
                // Status = unit.Status
            };
        }
    }

}

