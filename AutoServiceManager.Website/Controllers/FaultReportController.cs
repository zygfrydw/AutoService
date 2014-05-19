using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Website.Models;
using Microsoft.AspNet.Identity;


namespace AutoServiceManager.Website.Controllers
{
    [AuthorizeRole(ApplicationRoles.Customer)]
    public class FaultReportController : Controller
    {
        private DataContext db = new DataContext();
        

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                db.Dispose();
            base.Dispose(disposing);

        }

        public ActionResult Index()
        {
            
            var customer = User.GetCustomer(db);
            if (customer != null)
            {
               // db.Entry(customer).Collection(x => x.Cars).Load();
                IEnumerable<Car> userCars = customer.Cars;
                return View(userCars);
            }
            else
            {
                return View(new Car[]{});
            }
        }

        public ActionResult Report(long? id)
        {

            if (!id.HasValue)
                return RedirectToAction("Index");
            if (CarBellongToUser(id.Value))
            {
                var fault = new Fault() { CarID =  id.Value};
                return View(fault);    
            }
            return View("NotOwner");
        }

        [HttpPost]
        public ActionResult Report([Bind(Include = "CarID, Decription")]Fault fault)
        {
            if (ModelState.IsValid)
            {
                if (CarBellongToUser(fault.CarID))
                {
                    fault.IncomingDate = DateTime.Now;
                    fault.RepairStatus = Status.ReportedByCustomer;
                    db.Faults.Add(fault);
                    db.SaveChanges();
                    return View("Accepted", fault.ID);
                }
                return View("NotOwner");
            }
            else
            {
                return View(fault);
            }
        }

        private bool CarBellongToUser(long id)
        {
            var car = db.Cars.Find(id);
            if (car == null)
                return false;
            var customer = User.GetCustomer(db);
            return customer.Cars.Contains(car);
        }
    }
}