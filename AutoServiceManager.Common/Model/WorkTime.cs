using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Common.Model
{
    public class WorkTime
    {
        public long Id { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [DisplayName("Rozpoczęcie")]
        public DateTime StartTime { get; set; }
        [DisplayName("Zakończenie")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }
        [DisplayName("Stawka")]
        public decimal WorkerRate { get; set; }
        [DisplayName("Pracownik")]
        public long WorkerId { get; set; }
        public virtual Worker RelatedWorker { get; set; }
        [DisplayName("Opis pracy")]
        public string description { get; set; }
        public double WorkDuration
        {
            get { return (EndTime - StartTime).Days*24 + (EndTime - StartTime).Hours; }
        }

        public decimal WorkCost
        {
            get { return (decimal)WorkDuration*WorkerRate; }
        }

        public long FaultId { get; set; }
    }
}