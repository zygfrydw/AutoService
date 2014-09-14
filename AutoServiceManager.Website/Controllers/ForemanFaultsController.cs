using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Common.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace AutoServiceManager.Website.Controllers
{
    public class ForemanFaultsController : Controller
    {
        private DataContext db = new DataContext();

        class JsonSubfault
        {
            public string quantity { get; set; }
            public string name { get; set; }
            public string id { get; set; }
        }

        public ActionResult NewSubFault(String description, String parts, String parent)
        {
            PartOperation op;
            Worker assigned = db.Workers.FirstOrDefault(f => f.User.UserName == User.Identity.Name);
            var partsArray = new JavaScriptSerializer().Deserialize<JsonSubfault[]>(parts);
            SubFault subFault = new SubFault();
            subFault.Description = description;

            int tmpq;
            int.TryParse(parent, out tmpq);
            Fault parentFault = db.Faults.FirstOrDefault(f => f.ID == tmpq);
            subFault.ParentFault = parentFault;

            subFault.Worker = assigned;
            subFault.WorkerID = assigned.ID;

            List<Part> temp = new List<Part>();

            foreach (var part in partsArray)
            {
                var tempPart = new Part();
                
                int.TryParse(part.quantity, out tmpq);
                tempPart.Count = tmpq;

                int.TryParse(part.id, out tmpq);
                CataloguePart cataloguePart = db.CatalogueParts.FirstOrDefault(f => f.Id == tmpq);
                tempPart.PartFromCatalogue = cataloguePart;
                tempPart.CataloguePartId = cataloguePart.Id;
                temp.Add(tempPart);

                op = new PartOperation();

                op.WorkerId = assigned.ID;
                op.Worker = assigned;
                op.OperationType = PartOperationType.Request;
                op.PartId = tempPart.Id;
                op.Part = tempPart;

                decimal tmp = op.Part.PartFromCatalogue.Price;

                db.PartOperations.Add(op);
            }

            subFault.UsedParts=temp;

            db.SubFaults.Add(subFault);
            db.SaveChanges();

            int.TryParse(parent, out tmpq);
            return RedirectToAction("Edit", new { id = tmpq });
        }

        // GET: /ForemanFaults/
        public ActionResult Index()
        {
            var faults = db.Faults
                .Where(f => f.RepairStatus != Status.ReportedByCustomer && f.RepairStatus != Status.Relesed)
                .Include(f => f.Invoice)
                .Include(f => f.RelatedCar);
            ICollection<IdentityUserRole> users = db.Roles.FirstOrDefault(r => r.Name == ApplicationRoles.Worker.ToString()).Users;
            List<string> userIds = new List<string>();
            foreach (IdentityUserRole user in users)
            {
                userIds.Add(user.UserId);
            }
            ViewBag.Workers = db.People.Where(w => userIds.Contains(w.UserID) && w.Blocked != 1 && w.NotActive != 1).ToList();
            return View(faults.ToList());
        }

        // GET: /ForemanFaults/Edit/5
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
            ViewBag.Car = db.Cars.Include("Model").Include("Owner").FirstOrDefault(f => f.ID == fault.CarID);

            ICollection<IdentityUserRole> users = db.Roles.FirstOrDefault(r => r.Name == ApplicationRoles.Worker.ToString()).Users;

            List<string> userIds = new List<string>();
            foreach (IdentityUserRole user in users)
            {
                userIds.Add(user.UserId);
            }
            List<Person> workers = db.People.Where(w => userIds.Contains(w.UserID) && w.Blocked != 1 && w.NotActive != 1).ToList();
            IEnumerable<SelectListItem> workersView = 
                from worker in workers
                select new SelectListItem
                {
                    Text = worker.FirstName + " " + worker.SecondName,
                    Value = worker.ID.ToString(),
                    Selected = fault.WorkerID == worker.ID
                };
            ViewBag.Workers = workersView;

            List<SelectListItem> statuses = new List<SelectListItem>();
            foreach(Status status in (Status[])Enum.GetValues(typeof(Status)))
            {
                FieldInfo fi = status.GetType().GetField(status.ToString());
                DescriptionAttribute[] attributes = 
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                String description = (attributes.Length > 0) ? attributes[0].Description : status.ToString();

                if (status != Status.ReportedByCustomer && status != Status.Relesed)
                {
                    statuses.Add(new SelectListItem()
                    {
                        Text = description,
                        Value = ((int)status).ToString(),
                        Selected = status == fault.RepairStatus
                    });
                }
            }

            ViewBag.Statuses = statuses;

            return View(fault);
        }

        // POST: /ForemanFaults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,RepairStatus,IncomingDate,PredictedEndDate,CloseDate,RealeseDate,Decription,IncomeToService,WorkerID")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                var original = db.Faults.Find(fault.ID);
                
                if (original != null)
                {
                    fault.CarID = original.CarID;
                    fault.RelatedCar = original.RelatedCar;
                    fault.InvoiceID = original.InvoiceID;
                    fault.Invoice = original.Invoice;
                    db.Entry(original).CurrentValues.SetValues(fault);
                    db.SaveChanges();

                    if (fault.WorkerID != null)
                    {
                        Person newWorker = db.People.Find(fault.WorkerID);
                        if (!newWorker.Faults.Any(f => f.ID == fault.ID))
                        {
                            newWorker.Faults.Add(fault);
                        }
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("Index");
                }
            }
            return View(fault);
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
