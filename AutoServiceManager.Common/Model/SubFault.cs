using System.Collections.Generic;

namespace AutoServiceManager.Common.Model
{
    public class SubFault
    {
        public long Id { get; set; }
        public long FaultId { get; set; }
        public virtual Fault ParentFault { get; set; }
        public string Description { get; set; }
        public double RemainingHours { get; set; }
        public decimal EstimatedCost { get; set; }
        public virtual ICollection<WorkTime> WorkersHours { get; set; }
        public virtual ICollection<Part> UsedParts { get; set; }

    }
}