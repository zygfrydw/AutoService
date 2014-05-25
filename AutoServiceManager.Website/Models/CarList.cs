using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;
namespace AutoServiceManager.Website.Models
{
    public class CarList
    {
        private static DataContext db = new DataContext();

        public IEnumerable<Car> list { get; set; }
        public string query { get; set; }

        public CarList GetCarList(string query = null)
        {
             
            this.query=(query!=null)?query:this.query;
            
            if (this.query != null)
            {

                var found = (from c in db.Cars
                             where 
                             c.Model.ModelName == this.query ||
                             c.Model.Manufacturer.Name == this.query ||
                             c.Owner.FirstName == this.query ||
                             c.Owner.SecondName == this.query ||
                             c.RegistrationNumber== this.query
                            select c).ToList();
               list = (IEnumerable<AutoServiceManager.Common.Model.Car>)found;
            }
            else {
                list = db.Cars.ToList();
            }
            return this;
        }

        public static CarList GetAllCarList()
        {
            CarList createdObject = new CarList();
            createdObject.list = db.Cars.ToList();
            return createdObject;
        }
    }
}