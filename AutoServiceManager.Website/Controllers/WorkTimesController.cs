﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Website.Models;
using AutoServiceManager.Common.Identity;

namespace AutoServiceManager.Website.Controllers
{
    public class WorkTimesController : Controller
    {
        private DataContext db = new DataContext();

        [AuthorizeRole(ApplicationRoles.Worker, ApplicationRoles.Admin)]
        // GET: WorkTimes
        [HttpGet]
        public ActionResult Index()
        {
            var workedTime = db.WorkedTime.Include(w => w.RelatedWorker).Include(w => w.RelatedWorker.Faults);
            //var workedTime = db.WorkedTime.Include(w => w.RelatedWorker);
            
            if(User.IsInRole("Worker"))
            {
                Worker currentWorker = db.Workers.FirstOrDefault(f => f.User.UserName == User.Identity.Name);
                var worker = workedTime.Where(w => w.WorkerId == currentWorker.ID);
                ViewBag.Faults = currentWorker.Faults;
                return View(worker.ToList());
            }
            
            return View(workedTime.ToList());
        }


        // POST: WorkTimes
        [HttpPost]
        public ActionResult Index(String BlockedFilter, String year)
        {
            ViewBag.selectedMonth = BlockedFilter;
            ViewBag.selectedYear = year;

            var workedTime = db.WorkedTime.Include(w => w.RelatedWorker).Include(w => w.RelatedWorker.Faults);
            if (User.IsInRole("Worker"))
            {
                int temp = 0;
                int.TryParse(BlockedFilter, out temp);
                int intYear = 0;
                int.TryParse(year, out intYear);
                if (0 != intYear)
                {
                    var worker = workedTime
                        .Where(w => w.RelatedWorker.User.UserName == User.Identity.Name && w.StartTime.Month == temp && w.StartTime.Year == intYear);
                    var tmp = worker.ToList();
                    return View(worker.ToList());
                }
                else {
                    var worker = workedTime
                        .Where(w => w.RelatedWorker.User.UserName == User.Identity.Name && w.StartTime.Month == temp && w.StartTime.Year == DateTime.Now.Year);
                    return View(worker.ToList());
                }
                
            }
            return View(workedTime.ToList());

        }
        public ActionResult Order()
        {
            return View();
        }

        // GET: WorkTimes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkedTime.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }
            return View(workTime);
        }

        // GET: WorkTimes/Create
        public ActionResult Create()
        {
            List<Person> a = new List<Person>();
            //if (User.IsInRole("Worker"))
            //{
            //    foreach (var cos in db.People)
            //    {
            //        if (cos.User != null && cos.User.UserName == User.Identity.Name)
            //        {
            //            a.Add(cos);
            //            ViewBag.WorkerId = new SelectList(a, "ID", "FirstName");
            //            return View();
            //        }
            //    }
            //}

            //ViewBag.WorkerId = new SelectList(db.People, "ID", "FirstName");
            return View();
        }

        // POST: WorkTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartTime,EndTime,description,WorkerRate,WorkerId,FaultId")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                Worker currentWorker = db.Workers.FirstOrDefault(f => f.User.UserName == User.Identity.Name);
                workTime.RelatedWorker = currentWorker;
                workTime.WorkerId = currentWorker.ID;
                Fault fault = db.Faults.FirstOrDefault(f => f.ID == workTime.FaultId);
                workTime.Fault = fault;
                db.WorkedTime.Add(workTime);
                db.SaveChanges();
                return RedirectToAction("Details/"+workTime.Id.ToString());
            }

            ViewBag.WorkerId = new SelectList(db.People, "ID", "FirstName", workTime.WorkerId);
            return View(workTime);
        }

        // GET: WorkTimes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkedTime.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkerId = new SelectList(db.People, "ID", "FirstName", workTime.WorkerId);
            return View(workTime);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartTime,EndTime,description,WorkerRate,WorkerId")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkerId = new SelectList(db.People, "ID", "FirstName", workTime.WorkerId);
            return View(workTime);
        }

        // GET: WorkTimes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkedTime.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }
            return View(workTime);
        }

        // POST: WorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            WorkTime workTime = db.WorkedTime.Find(id);
            db.WorkedTime.Remove(workTime);
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
