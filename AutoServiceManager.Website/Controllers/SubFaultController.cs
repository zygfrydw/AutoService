using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;
using System.Reflection;
using System.ComponentModel;

namespace AutoServiceManager.Website.Controllers
{
    public class SubFaultController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /SubFault/
        public ActionResult Index()
        {
            var subfaults = db.SubFaults.Include(s => s.ParentFault).Include(s => s.Worker);
            return View(subfaults.ToList());
        }

        // GET: /SubFault/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFault subfault = db.SubFaults.Find(id);
            if (subfault == null)
            {
                return HttpNotFound();
            }
            return View(subfault);
        }

        // GET: /SubFault/Create
        public ActionResult Create()
        {
            ViewBag.FaultId = new SelectList(db.Faults, "ID", "Decription");
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName");
            return View();
        }

        // POST: /SubFault/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,FaultId,Description,RemainingHours,EstimatedCost,WorkerID")] SubFault subfault)
        {
            if (ModelState.IsValid)
            {
                db.SubFaults.Add(subfault);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FaultId = new SelectList(db.Faults, "ID", "Decription", subfault.FaultId);
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName", subfault.WorkerID);
            return View(subfault);
        }

        // GET: /SubFault/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFault subfault = db.SubFaults.Find(id);
            if (subfault == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> statuses = new List<SelectListItem>();
            foreach (SubFaultStatus status in (SubFaultStatus[])Enum.GetValues(typeof(SubFaultStatus)))
            {
                FieldInfo fi = status.GetType().GetField(status.ToString());
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                String description = (attributes.Length > 0) ? attributes[0].Description : status.ToString();

                if (status != SubFaultStatus.Rejected)
                {
                    statuses.Add(new SelectListItem()
                    {
                        Text = description,
                        Value = ((int)status).ToString(),
                        Selected = status == subfault.Status
                    });
                }
            }

            ViewBag.Statuses = statuses;

            return View(subfault);
        }

        // POST: /SubFault/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Description,Status")] SubFault subfault)
        {
            if (ModelState.IsValid)
            {
                SubFault original = db.SubFaults.Find(subfault.Id);

                if (original != null)
                {
                    original.Status = subfault.Status;
                    original.Description = subfault.Description;

                    db.Entry(original);
                    db.SaveChanges();
                }
                //Redirect("~/ForemanFaults/Edit/"+subfault.FaultId)
                return RedirectToAction("Edit/" + original.FaultId.ToString(), "ForemanFaults");
            }
            return View(subfault);
        }

        public ActionResult Accept(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubFault original = db.SubFaults.Find(id);
            if (original == null)
            {
                return HttpNotFound();
            }

            original.Status = SubFaultStatus.Accepted;

            db.Entry(original);
            db.SaveChanges();

            return RedirectToAction("Edit/" + original.FaultId.ToString(), "ForemanFaults");
        }

        public ActionResult Reject(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubFault original = db.SubFaults.Find(id);
            if (original == null)
            {
                return HttpNotFound();
            }

            original.Status = SubFaultStatus.Rejected;

            db.Entry(original);
            db.SaveChanges();

            return RedirectToAction("Edit/" + original.FaultId.ToString(), "ForemanFaults");
        }

        // GET: /SubFault/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubFault subfault = db.SubFaults.Find(id);
            if (subfault == null)
            {
                return HttpNotFound();
            }
            return View(subfault);
        }

        // POST: /SubFault/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SubFault subfault = db.SubFaults.Find(id);
            db.SubFaults.Remove(subfault);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept([Bind(Include = "Id,FaultId,Description,RemainingHours,EstimatedCost,WorkerID")] SubFault subfault)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subfault).State = EntityState.Modified;
                db.SaveChanges();
                //Redirect("~/ForemanFaults/Edit/"+subfault.FaultId)
                return RedirectToAction("Edit/" + subfault.FaultId.ToString(), "ForemanFaults");
            }
            ViewBag.FaultId = new SelectList(db.Faults, "ID", "Decription", subfault.FaultId);
            ViewBag.WorkerID = new SelectList(db.People, "ID", "FirstName", subfault.WorkerID);
            return View(subfault);
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
