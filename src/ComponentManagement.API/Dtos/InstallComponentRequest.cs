using System;

namespace ComponentManagement.API.Dtos
{
    public class InstallComponentRequest
    {
        // INPUT MANUAL PLANNER / PE
        public double TotalLifetimeHm { get; set; }

        public DateTimeOffset InstalledAt { get; set; }
    }
}
