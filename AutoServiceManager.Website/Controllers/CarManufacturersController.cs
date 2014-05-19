using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Website.Models;

namespace AutoServiceManager.Website.Controllers
{
    [AuthorizeRole(ApplicationRoles.Secretary)]
    public class CarManufacturersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /CarManufacturers/
        public ActionResult Index()
        {
            return View(db.CarManufacturers.ToList());
        }

        // GET: /CarManufacturers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarManufacturer carmanufacturer = db.CarManufacturers.Find(id);
            if (carmanufacturer == null)
            {
                return HttpNotFound();
            }
            return View(carmanufacturer);
        }

        // GET: /CarManufacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CarManufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name")] CarManufacturer carmanufacturer)
        {
            if (ModelState.IsValid)
            {
                db.CarManufacturers.Add(carmanufacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carmanufacturer);
        }

        // GET: /CarManufacturers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarManufacturer carmanufacturer = db.CarManufacturers.Find(id);
            if (carmanufacturer == null)
            {
                return HttpNotFound();
            }
            return View(carmanufacturer);
        }

        // POST: /CarManufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name")] CarManufacturer carmanufacturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carmanufacturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carmanufacturer);
        }

        // GET: /CarManufacturers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarManufacturer carmanufacturer = db.CarManufacturers.Find(id);
            if (carmanufacturer == null)
            {
                return HttpNotFound();
            }
            return View(carmanufacturer);
        }

        // POST: /CarManufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CarManufacturer carmanufacturer = db.CarManufacturers.Find(id);
            db.CarManufacturers.Remove(carmanufacturer);
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
