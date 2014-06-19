using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Mvc;

using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Models
{
    public class FaultList
    {
        private DataContext db = new DataContext();

        public string query { get; set; }
        public string statusQuery { get; set; }
        public string order { get; set; }
        public IEnumerable<Fault> list { get; set; }


        public FaultList GetFaultList()
        {
            var Found = db.Faults.Include("RelatedCar").AsQueryable();

            if (!string.IsNullOrEmpty(this.query))
            {
                int tryID = 0;
                if(!int.TryParse(this.query,out tryID)){
                    tryID = -1;
                }

                Found = Found.Where( c =>
                        c.ID == tryID ||
                        c.Decription.Contains(this.query) ||
                        c.RelatedCar.Model.ModelName.Contains(this.query) ||
                        c.RelatedCar.Model.Manufacturer.Name.Contains(this.query) ||
                        c.RelatedCar.Owner.FirstName.Contains(this.query) ||
                        c.RelatedCar.Owner.SecondName.Contains(this.query) 
                        
                ).AsQueryable();
            }

            if (!string.IsNullOrEmpty(this.statusQuery))
            {
                int statusQueyInt = 0;
                if (int.TryParse(this.statusQuery,out statusQueyInt)) {
                    Found = Found.Where(c => c.RepairStatus == (Status)statusQueyInt);
                }
            }

            if (this.order == "Status")
            {
                Found = Found.OrderBy(c => c.RepairStatus);
            }
            else if (this.order == "Model")
            {
                Found = Found.OrderBy(c => c.RelatedCar.Model.ModelName);
            }
            else if (this.order == "Manufacturer")
            {
                Found = Found.OrderBy(c => c.RelatedCar.Model.Manufacturer.Name);
            }
            else if (this.order == "IncomingDate")
            {
                Found = Found.OrderBy(c => c.IncomingDate);
            }
            else if (this.order == "PredictedEndDate")
            {
                Found = Found.OrderBy(c => c.PredictedEndDate);
            }
            else if (this.order == "CloseDate")
            {
                Found = Found.OrderBy(c => c.CloseDate);
            }
            else if (this.order == "RealeseDate")
            {
                Found = Found.OrderBy(c => c.RealeseDate);
            }
            

            this.list = Found.ToList();
            return this;
        }

        public static FaultList GetAllFaultList()
        {
            FaultList createdObject = new FaultList();
            return createdObject.GetFaultList();
        }
    }
}