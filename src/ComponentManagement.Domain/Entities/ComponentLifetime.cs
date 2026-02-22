using ComponentManagement.Domain.Exceptions;

namespace ComponentManagement.Domain.Entities
{
    public class ComponentLifetime
    {
        public Guid Id { get; set; }

        public Guid PartComponentId { get; set; }
        public PartComponent PartComponent { get; set; }

        public double TotalLifetimeHm { get; set; }
        public DateTimeOffset? InstalledAt { get; set; }

        // cache (optional tapi bagus)
        public double UsedLifetimeHm { get; set; }
        public double RemainingLifetimeHm { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // anti-spam notification
        public string NotifiedThresholds { get; private set; } = "";

        public bool HasNotified(int threshold)
        {
            return NotifiedThresholds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Any(x => x == threshold.ToString());
        }

        public void MarkNotified(int threshold)
        {
            if (!HasNotified(threshold))
                NotifiedThresholds += $"{threshold},";
        }

        public double CalculateUsedHm(DateTimeOffset now)
        {
            if (!InstalledAt.HasValue) return 0;
            var duration = now - InstalledAt.Value;
            return Math.Max(0, Math.Round(duration.TotalHours, 2));
        }

        public double CalculateRemainingHm(DateTimeOffset now)
        {
            var usedHm = CalculateUsedHm(now);
            return Math.Max(0, Math.Round(TotalLifetimeHm - usedHm, 2));
        }

    }
}
