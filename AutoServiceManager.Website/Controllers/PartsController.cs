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
    public class PartsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Parts
        public ActionResult Index()
        {
            var usedParts = db.UsedParts.Include(p => p.PartFromCatalogue);
            return View(usedParts.ToList());
        }

        public ActionResult Partsapi()
        {
            List<object> list = new List<object>();
            foreach (var obj in db.CatalogueParts.ToArray())
            {
                list.Add(new
                {
                 id = obj.Id,
                 name = obj.PartName
                });
            }

            return Json(list,JsonRequestBehavior.AllowGet);
        }

        // GET: Parts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.UsedParts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // GET: Parts/Create
        public ActionResult Create()
        {
            ViewBag.CataloguePartId = new SelectList(db.CatalogueParts, "Id", "PartName");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CataloguePartId,SoldPrice,Count")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.UsedParts.Add(part);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CataloguePartId = new SelectList(db.CatalogueParts, "Id", "PartName", part.CataloguePartId);
            return View(part);
        }

        // GET: Parts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.UsedParts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            ViewBag.CataloguePartId = new SelectList(db.CatalogueParts, "Id", "PartName", part.CataloguePartId);
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CataloguePartId,SoldPrice,Count")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CataloguePartId = new SelectList(db.CatalogueParts, "Id", "PartName", part.CataloguePartId);
            return View(part);
        }

        // GET: Parts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.UsedParts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Part part = db.UsedParts.Find(id);
            db.UsedParts.Remove(part);
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
