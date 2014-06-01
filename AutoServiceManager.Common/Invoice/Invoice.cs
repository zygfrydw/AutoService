using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Common.Invoice
{
    public class Invoice
    {
        public Car Car { get; set; }
        public Customer Customer { get; set;}
        public string City { get; set; }
        public string Country { get; set; }
        public IEnumerable<Part> Parts { get; set; }
        public double WorkedHours { get; set; }
        [DataType(DataType.Currency)]
        public decimal WorkCosts { get; set; }
        [DataType(DataType.Currency)]
        public decimal ExtraCosts { get; set; }
        [DataType(DataType.Currency)]
        public decimal PartCosts { get; set; }
        public long FaultId { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalCost
        {
            get { return WorkCosts + PartCosts + ExtraCosts; }
        }
    }
}