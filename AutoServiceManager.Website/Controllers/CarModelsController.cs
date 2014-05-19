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
    public class CarModelsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /CarModels/
        public ActionResult Index()
        {
            return View(db.CarModels.ToList());
        }

        // GET: /CarModels/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carmodel = db.CarModels.Find(id);
            if (carmodel == null)
            {
                return HttpNotFound();
            }
            return View(carmodel);
        }

        // GET: /CarModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,ModelName")] CarModel carmodel)
        {
            if (ModelState.IsValid)
            {
                db.CarModels.Add(carmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carmodel);
        }

        // GET: /CarModels/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carmodel = db.CarModels.Find(id);
            if (carmodel == null)
            {
                return HttpNotFound();
            }
            return View(carmodel);
        }

        // POST: /CarModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,ModelName")] CarModel carmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carmodel);
        }

        // GET: /CarModels/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carmodel = db.CarModels.Find(id);
            if (carmodel == null)
            {
                return HttpNotFound();
            }
            return View(carmodel);
        }

        // POST: /CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CarModel carmodel = db.CarModels.Find(id);
            db.CarModels.Remove(carmodel);
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
