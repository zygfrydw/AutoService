using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoServiceManager.Common.DataAnnotations;
using AutoServiceManager.Website.Models;

namespace AutoServiceManager.Common.Model
{
    public enum Status
    {
        [Description("Zgłoszenie oczekuje na zatwierdzenie")]
        [Color("red")]
        [HtmlClass("danger")]
        ReportedByCustomer,
        [Description("Zgłoszenie zostało przyjęte")]
        [Color("red")]
        [HtmlClass("warning")]
        Approved,
        [Description("Samochód został przyjęty do serwisu")]
        [Color("red")]
        [HtmlClass("info")]
        NewInService,
        [Description("W trakcie naprawy")]
        [Color("#FF9966")]
        [HtmlClass("info")]
        InProgress,
        [Description("Naprawa zakończona")]
        [Color("green")]
        [HtmlClass("success")]
        Done,
        [Description("Odebrane przez klienta")]
        [Color("green")]
        [HtmlClass("success")]
        Relesed
    }

    public class Fault
    {
       

        public long ID { get; set; }
        [Required]
        [DisplayName("Auto")]
        public long CarID { get; set; }
        [DisplayName("Auto")]
        public virtual Car RelatedCar { get; set; }
        [DisplayName("Status Naprawy")]
        public Status RepairStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data zgłoszenia")]
        public DateTime IncomingDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Przewidywana data zakończenia")]
        public DateTime? PredictedEndDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data zakończenia")]
        public DateTime? CloseDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data wydania")]
        public DateTime? RealeseDate { get; set; }

        [Required]
        [MinLength(10)]
        [DisplayName("Opis")]
        public string Decription { get; set; }
        public virtual ICollection<SubFault> SubFaults { get; set; }
        public DateTime? IncomeToService { get; set; }

        public long? InvoiceID { get; set; }
        public Invoice Invoice { get;set; }

    }
}
