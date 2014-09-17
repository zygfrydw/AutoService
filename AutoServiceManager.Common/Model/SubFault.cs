using AutoServiceManager.Common.DataAnnotations;
using AutoServiceManager.Website.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutoServiceManager.Common.Model
{
    public enum SubFaultStatus
    {
        [Description("Zg�oszenie oczekuje na zatwierdzenie")]
        [Color("red")]
        [HtmlClass("danger")]
        ReportedByCustomer,
        [Description("Zg�oszenie zosta�o przyj�te")]
        [Color("red")]
        [HtmlClass("warning")]
        Approved,
        [Description("Samoch�d zosta� przyj�ty do serwisu")]
        [Color("red")]
        [HtmlClass("info")]
        NewInService,
        [Description("W trakcie naprawy")]
        [Color("#FF9966")]
        [HtmlClass("info")]
        InProgress,
        [Description("Naprawa zako�czona")]
        [Color("green")]
        [HtmlClass("success")]
        Done,
        [Description("Odebrane przez klienta")]
        [Color("green")]
        [HtmlClass("success")]
        Relesed
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