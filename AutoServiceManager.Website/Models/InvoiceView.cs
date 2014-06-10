using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Linq.Dynamic;
using AutoServiceManager.Common.Model;
namespace AutoServiceManager.Website.Models
{
    public class InvoiceView
    {
        private DataContext db = new DataContext();

        public List<SubFault> ListOfSubFaults { get; set; }
        public List<WorkTime> ListOfWorkTimes { get; set; }
        public List<Part> ListOfParts { get; set; }
        

        public Invoice Invoice { get; set; }

        public InvoiceView() {
                this.ListOfSubFaults = new List<SubFault>();
                this.ListOfWorkTimes = new List<WorkTime>();
                this.ListOfParts = new List<Part>();
        }

        public decimal getSumForListOfSubFaults()
        {
            decimal sum = (decimal)0;
            if (this.ListOfSubFaults != null)
            {
                foreach (SubFault SubFault in this.ListOfSubFaults)
                {
                    sum += SubFault.EstimatedCost;
                }
            }
            return sum;
        }

        public decimal getSumForListOfWorkTimes(){
            decimal sum = (decimal)0;
            if (this.ListOfWorkTimes != null)
            {
                foreach (WorkTime WorkTime in this.ListOfWorkTimes)
                {
                    sum += WorkTime.WorkCost;
                }
            }
            return sum;
        }

        public decimal getSumForListOfParts()
        {
            decimal sum = (decimal)0;
            if (this.ListOfParts!=null)
            {
                foreach (Part Part in this.ListOfParts)
                {
                    sum += Part.Cost;
                } 
            }
            return sum;
        }

        private decimal vat = 23;

        public decimal NettoToBrutto(decimal x){
            return x+(x * (this.vat / 100));
        }

        protected Fault getFault(int FaultID){
            return db.Faults
                .Include("Invoice")
                .Include("SubFaults")
                .Include("SubFaults.UsedParts")
                .Include("SubFaults.UsedParts.PartFromCatalogue")
                .Include("SubFaults.WorkersHours")
                .FirstOrDefault(f => f.ID == FaultID);
        }

        public InvoiceView(int FaultID) {
            Fault Fault = getFault(FaultID);
            if (Fault == null)
            {
                return;
            }
            if (Fault.Invoice == null)
            {
                Fault.Invoice = new Invoice();
                db.Invoice.Add(Fault.Invoice);
                db.SaveChanges();
                Fault = getFault(FaultID);
            }
        
            this.Invoice = Fault.Invoice;
            this.ListOfWorkTimes = new List<WorkTime>();
            this.ListOfParts = new List<Part>();

            this.ListOfSubFaults = (Fault.SubFaults != null) ? Fault.SubFaults.ToList() : new List<SubFault>();

            foreach (SubFault SubFault in this.ListOfSubFaults)
            {
                if (SubFault.UsedParts != null) {
                    this.ListOfParts.AddRange(SubFault.UsedParts.ToList());
                }
                if (SubFault.WorkersHours != null)
                {
                    this.ListOfWorkTimes.AddRange(SubFault.WorkersHours.ToList());
                }
            }
            
        }

    }
}