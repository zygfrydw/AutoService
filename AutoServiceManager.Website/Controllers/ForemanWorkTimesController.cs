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
    public class ForemanWorkTimesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /ForemanWorkTimes/
        public ActionResult Index()
        {
            var workedtime = db.WorkedTime.Include(w => w.Fault).Include(w => w.RelatedWorker).Where(w => w.WorkerRate == 0);
            return View(workedtime.ToList());
        }

        // GET: /ForemanWorkTimes/Rates
        public ActionResult Rates()
        {
            var workedtime = db.WorkedTime.Include(w => w.Fault).Include(w => w.RelatedWorker).Where(w => w.WorkerRate != 0);
            return View(workedtime.ToList());
        }

        // GET: /ForemanWorkTimes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime worktime = db.WorkedTime.Find(id);
            if (worktime == null)
            {
                return HttpNotFound();
            }
            return View(worktime);
        }

        // POST: /ForemanWorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,WorkerRate")] WorkTime worktime)
        {
            WorkTime original = db.WorkedTime.Find(worktime.Id);
            if (ModelState.IsValid)
            {
                original.WorkerRate = worktime.WorkerRate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(worktime);
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
