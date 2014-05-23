using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Website.Models;

namespace AutoServiceManager.Website.Controllers
{
    [AuthorizeRole(ApplicationRoles.Customer)]
    public class FaultReportController : Controller
    {
        private readonly DataContext db = new DataContext();


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            Customer customer = User.GetCustomer(db);
            if (customer != null)
            {
                // db.Entry(customer).Collection(x => x.Cars).Load();
                IEnumerable<Car> userCars = customer.Cars;
                return View(userCars);
            }
            return View(new Car[] {});
        }

        public ActionResult Report(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            if (CarBellongToUser(id.Value))
            {
                var fault = new Fault {CarID = id.Value};
                return View(fault);
            }
            return View("NotOwner");
        }

        [HttpPost]
        public ActionResult Report([Bind(Include = "CarID, Decription")] Fault fault)
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
            return View(fault);
        }

        public ActionResult Edit(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            if (CarBellongToUser(id.Value))
            {
                try
                {
                    var car = db.Cars.Find(id.Value);
                    var editedFault = (from fault in car.Faults
                        where fault.RepairStatus == Status.ReportedByCustomer && fault.IncomingDate == (car.Faults.Max(f => f.IncomingDate))
                        select fault).First();
                    ModelState.Clear();
                    return View(editedFault);
                }
                catch 
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            return View("NotOwner");
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Decription, ID")] Fault fault)
        {
            if (ModelState.IsValid)
            {

                        var originalFault = db.Faults.Find(fault.ID);
                        if(originalFault == null)
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        if (CarBellongToUser(originalFault.CarID))
                        {
                            originalFault.Decription = fault.Decription;
                            originalFault.IncomingDate = DateTime.Now;
                            db.Entry(originalFault).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            return View("NotOwner");
                        }
                        return View("Changed", fault.ID);
                    
                
            }
            return View(fault);
        }

        private bool CarBellongToUser(long id)
        {
            Car car = db.Cars.Find(id);
            if (car == null)
                return false;
            Customer customer = User.GetCustomer(db);
            return customer.Cars.Contains(car);
        }
    }
}