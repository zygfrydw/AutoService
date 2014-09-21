using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Controllers
{
    public class WorkerFaultsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /WorkerFaults/
        public ActionResult Index()
        {
            Worker currentWorker = db.Workers.FirstOrDefault(f => f.User.UserName == User.Identity.Name);
            var faults = db.Faults.Include(f => f.Invoice).Include(f => f.RelatedCar).Include(f => f.Worker).Where(f => f.WorkerID == currentWorker.ID);

            return View(faults.ToList());
        }

        // GET: /WorkerFaults/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fault fault = db.Faults.Find(id);
            if (fault == null)
            {
                return HttpNotFound();
            }
            return View(fault);
        }

        // GET: /WorkerFaults/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceID = new SelectList(db.Invoice, "ID", "ID");
            ViewBag.CarID = new SelectList(db.Cars, "ID", "RegistrationNumber");
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName");
            return View();
        }

        // POST: /WorkerFaults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,CarID,RepairStatus,IncomingDate,PredictedEndDate,CloseDate,RealeseDate,Decription,IncomeToService,InvoiceID,WorkerID")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                db.Faults.Add(fault);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceID = new SelectList(db.Invoice, "ID", "ID", fault.InvoiceID);
            ViewBag.CarID = new SelectList(db.Cars, "ID", "RegistrationNumber", fault.CarID);
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName", fault.WorkerID);
            return View(fault);
        }

        // GET: /WorkerFaults/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fault fault = db.Faults.Find(id);
            if (fault == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceID = new SelectList(db.Invoice, "ID", "ID", fault.InvoiceID);
            ViewBag.CarID = new SelectList(db.Cars, "ID", "RegistrationNumber", fault.CarID);
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName", fault.WorkerID);
            return View(fault);
        }

        // POST: /WorkerFaults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,CarID,RepairStatus,IncomingDate,PredictedEndDate,CloseDate,RealeseDate,Decription,IncomeToService,InvoiceID,WorkerID")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fault).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceID = new SelectList(db.Invoice, "ID", "ID", fault.InvoiceID);
            ViewBag.CarID = new SelectList(db.Cars, "ID", "RegistrationNumber", fault.CarID);
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName", fault.WorkerID);
            return View(fault);
        }

        // GET: /WorkerFaults/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fault fault = db.Faults.Find(id);
            if (fault == null)
            {
                return HttpNotFound();
            }
            return View(fault);
        }

        // POST: /WorkerFaults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Fault fault = db.Faults.Find(id);
            db.Faults.Remove(fault);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
