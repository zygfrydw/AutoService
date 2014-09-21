using AutoServiceManager.Common.DataAnnotations;
using AutoServiceManager.Website.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutoServiceManager.Common.Model
{
    public enum SubFaultStatus
    {
        [Description("W trakcie naprawy")]
        [Color("red")]
        [HtmlClass("warning")]
        InProgress,
        [Description("Naprawa zakoñczona")]
        [Color("red")]
        [HtmlClass("success")]
        Done,
        [Description("Odrzucone przez majstra")]
        [Color("red")]
        [HtmlClass("error")]
        Rejected,
        [Description("Zaakceptowane")]
        [Color("green")]
        [HtmlClass("success")]
        Accepted
        
    }
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
        public long? WorkerID { get; set; }
        public virtual Worker Worker { get; set; }

        public long PartOperationId { get; set; }

        public SubFaultStatus Status { get; set; }
    }
}