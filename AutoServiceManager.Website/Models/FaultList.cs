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
        private static DataContext db = new DataContext();

        public IEnumerable<Fault> list { get; set; }
        public string query { get; set; }
        /*
        public CarList GetCarList()
        {
            if (query != null)
            {

                var found = (from c in db.Cars
                             where 
                             c.Model.ModelName == query ||
                             c.Model.Manufacturer.Name == query ||
                             c.Owner.FirstName == query ||
                             c.Owner.SecondName == query ||
                             c.RegistrationNumber== query
                            select c).ToList();
               list = (IEnumerable<AutoServiceManager.Common.Model.Car>)found;
            }
            else {
                list = db.Cars.ToList();
            }
            return this;
        }
        */
        public static FaultList GetAllFaultList()
        {
            FaultList createdObject = new FaultList();
            createdObject.list = db.Faults.Include("RelatedCar").ToList();
            return createdObject;
        }
    }
}