using System.Collections.Generic;

namespace AutoServiceManager.Common.Model
{
    public class SubFault
    {
        public long Id { get; set; }
        
        public Fault ParentFault { get; set; }

        public string Description { get; set; }
        public double RemainingHours { get; set; }
        public decimal EstimatedCost { get; set; }
        public ICollection<WorkTime> WorkersHours { get; set; }
        public ICollection<Part> UsedParts { get; set; }

    }
}