using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Linq.Dynamic;
using AutoServiceManager.Common.Model;
namespace AutoServiceManager.Website.Models
{
    public class CarList
    {
        private  DataContext db = new DataContext();

        public IEnumerable<Car> list { get; set; }
        public string query { get; set; }
        public string order { get; set; }

        public string highlightDifferentUser { get; set; }

        public CarList GetCarList(string query = null)
        {
            DataContext db = new DataContext();

            this.query=(query!=null)?query:this.query;

            var Found = db.Cars.Include("Owner").Include("Owner.Address").Include("Owner.Address.Country").Include("Owner.Address.City").AsQueryable(); 
            
            if (!string.IsNullOrEmpty(this.query))
            {
                int productionYear = 0;
                int.TryParse(this.query,out productionYear);

                Found = Found.Where(c =>
                        c.ProductionYear.Equals(productionYear) ||
                        c.Model.ModelName.Contains(this.query) ||
                        c.Model.Manufacturer.Name.Contains(this.query) ||
                        c.Owner.FirstName.Contains(this.query) ||
                        c.Owner.SecondName.Contains(this.query) ||
                        c.RegistrationNumber.Contains(this.query)
                        ).AsQueryable();
            }
            if (this.order == "ModelName") { 
                Found = Found.OrderBy(c => c.Model.ModelName);
            }
            else if (this.order == "ManufacturerName")
            {
                Found = Found.OrderBy(c => c.Model.Manufacturer.Name);
            }
            else if (this.order == "OwnerFirstName")
            {
                Found = Found.OrderBy(c => c.Owner.FirstName);
            }
            else if (this.order == "OwnerSecondName")
            {
                Found = Found.OrderBy(c => c.Owner.SecondName);
            }
            else if (this.order == "RegistrationNumber")
            {
                Found = Found.OrderBy(c => c.RegistrationNumber);
            }
            else if (this.order == "ProductionYear")
            {
                Found = Found.OrderBy(c => c.ProductionYear);
            }
            

            list = (IEnumerable<AutoServiceManager.Common.Model.Car>)Found.ToList();

            return this;
        }

        public static CarList GetAllCarList()
        {
            CarList createdObject = new CarList();
            return createdObject.GetCarList();
        }
    }
}