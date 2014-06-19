using System;

namespace AutoServiceManager.Common.Model
{
    public class WorkTime
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal WorkerRate { get; set; }
        public long WorkerId { get; set; }
        public virtual Worker RelatedWorker { get; set; }

        public double WorkDuration
        {
            get { return (EndTime - StartTime).Hours; }
        }

        public decimal WorkCost
        {
            get { return (decimal)WorkDuration*WorkerRate; }
        }
    }
}