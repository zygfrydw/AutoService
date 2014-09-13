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
    [AuthorizeRole(ApplicationRoles.Storekeeper)]
    public class CataloguePartController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /CataloguePart/
        public ActionResult Index()
        {
            return View(db.CatalogueParts.ToList());
        }

        // GET: /CataloguePart/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CataloguePart cataloguepart = db.CatalogueParts.Find(id);
            if (cataloguepart == null)
            {
                return HttpNotFound();
            }
            return View(cataloguepart);
        }

        // GET: /CataloguePart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CataloguePart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PartName,SerialNumber,Price,InStock")] CataloguePart cataloguepart)
        {
            if (ModelState.IsValid)
            {
                db.CatalogueParts.Add(cataloguepart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cataloguepart);
        }

        // GET: /CataloguePart/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CataloguePart cataloguepart = db.CatalogueParts.Find(id);
            if (cataloguepart == null)
            {
                return HttpNotFound();
            }
            return View(cataloguepart);
        }

        // POST: /CataloguePart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PartName,SerialNumber,Price,InStock")] CataloguePart cataloguepart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cataloguepart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cataloguepart);
        }

        // GET: /CataloguePart/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CataloguePart cataloguepart = db.CatalogueParts.Find(id);
            if (cataloguepart == null)
            {
                return HttpNotFound();
            }
            return View(cataloguepart);
        }

        // POST: /CataloguePart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CataloguePart cataloguepart = db.CatalogueParts.Find(id);
            db.CatalogueParts.Remove(cataloguepart);
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
