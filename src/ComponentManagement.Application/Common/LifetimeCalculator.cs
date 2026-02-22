using System;

namespace ComponentManagement.Application.Common
{
    public static class LifetimeCalculator
    {
        public static double CalculateUsedHm(DateTimeOffset installedAt)
        {
            return Math.Round(
                (DateTimeOffset.UtcNow - installedAt).TotalHours, 2);
        }

        public static double CalculateRemainingHm(
            double totalHm,
            DateTimeOffset installedAt)
        {
            var used = CalculateUsedHm(installedAt);
            return Math.Round(Math.Max(totalHm - used, 0), 2);
        }
    }
}
