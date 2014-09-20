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
    public class PartOperationsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /PartOperations/
        public ActionResult Index()
        {
            var partoperations = db.PartOperations
                .Where(p => p.OperationType == PartOperationType.Approved)
                .Include(p => p.Part)
                .Include(p => p.Worker)
                .Include(p => p.Part.PartFromCatalogue);

            return View(partoperations.ToList());
        }

        // GET: /PartOperations/RejectedParts
        public ActionResult RejectedParts()
        {
            var partoperations = db.PartOperations
                .Where(p => p.OperationType == PartOperationType.Rejected)
                .Include(p => p.Part)
                .Include(p => p.Worker)
                .Include(p => p.Part.PartFromCatalogue);

            return View(partoperations.ToList());
        }

        // GET: /PartOperations/RequestedParts
        public ActionResult RequestedParts()
        {
            var partoperations = db.PartOperations
                .Where(p => p.OperationType == PartOperationType.Request)
                .Include(p => p.Part)
                .Include(p => p.Worker)
                .Include(p => p.Part.PartFromCatalogue);

            return View(partoperations.ToList());
        }

        // GET: /PartOperations/ReleasedParts
        public ActionResult ReleasedParts()
        {
            var partoperations = db.PartOperations
                .Where(p => p.OperationType == PartOperationType.Released)
                .Include(p => p.Part)
                .Include(p => p.Worker)
                .Include(p => p.Part.PartFromCatalogue);

            return View(partoperations.ToList());
        }

        public ActionResult ReleaseNotEnough(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartOperation partoperation = db.PartOperations.Find(id);
            if (partoperation == null)
            {
                return HttpNotFound();
            }

            if (partoperation.OperationType == PartOperationType.Released)
            {
                return HttpNotFound();
            }
            return View(partoperation);
        }

        public ActionResult Release(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartOperation partoperation = db.PartOperations.Find(id);
            if (partoperation == null)
            {
                return HttpNotFound();
            }

            if (partoperation.OperationType == PartOperationType.Released)
            {
                return HttpNotFound();
            }
            if (partoperation.Part.Count > partoperation.Part.PartFromCatalogue.InStock)
            {
                return RedirectToAction("ReleaseNotEnough/" + id.ToString());
            }
            partoperation.OperationType = PartOperationType.Released;
            partoperation.Part.PartFromCatalogue.InStock -= partoperation.Part.Count;
            db.SaveChanges();

            return View(partoperation);
        }

        public ActionResult Approve(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartOperation partoperation = db.PartOperations.Find(id);
            if (partoperation == null)
            {
                return HttpNotFound();
            }

            if (partoperation.OperationType != PartOperationType.Request)
            {
                return HttpNotFound();
            }
            partoperation.OperationType = PartOperationType.Approved;
            db.SaveChanges();

            return View(partoperation);
        }

        public ActionResult Reject(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartOperation partoperation = db.PartOperations.Find(id);
            if (partoperation == null)
            {
                return HttpNotFound();
            }

            if (partoperation.OperationType != PartOperationType.Request)
            {
                return HttpNotFound();
            }
            Worker assigned = db.Workers.FirstOrDefault(f => f.User.UserName == User.Identity.Name);
            partoperation.OperationType = PartOperationType.Rejected;
            partoperation.RejectingWorkerId = assigned.ID;
            partoperation.RejectingWorker = assigned;
            db.SaveChanges();

            return View(partoperation);
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
