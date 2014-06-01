using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Invoice;
using AutoServiceManager.Common.Model;
using AutoServiceManager.Website.Models;
using AutoServiceManager.Website.Models.FilteringModels;

namespace AutoServiceManager.Website.Controllers
{
    [AuthorizeRole(ApplicationRoles.Customer)]
    public class RepairStatusController : Controller
    {
        DataContext db =new DataContext();
        //
        // GET: /RepairStatus/
        public ActionResult Index()
        {
            var model = new CustomerFaultsPreviewModel();
            model.Cars = GetPreviewModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CustomerFaultsPreviewModel previewModel)
        {
            if (previewModel == null)
                previewModel = new CustomerFaultsPreviewModel();
            previewModel.Cars = GetPreviewModel(previewModel);
            return View(previewModel);
        }

        public ActionResult CustomerCarsPreview(CustomerFaultsPreviewModel previewModel)
        {
            if (previewModel == null)
                previewModel = new CustomerFaultsPreviewModel();
            var cars = GetPreviewModel(previewModel);
            return PartialView(cars);
        }

        private List<RepairStatusViewModel> GetPreviewModel(CustomerFaultsPreviewModel previewModel)
        {
            var customer = User.GetCustomer(db);
            var customerCars = db.Cars.Where(x => x.OwnerID == customer.ID).Include(x => x.Faults).Include(x => x.Model);
            if (previewModel.Manufacturer != null)
            {
                customerCars = customerCars.Where(x => x.Model.Manufacturer.Name.Contains(previewModel.Manufacturer));
            }
            if (previewModel.Model != null)
            {
                customerCars = customerCars.Where(x => x.Model.ModelName.Contains(previewModel.Model));
            }
            if (previewModel.RegisterNumber != null)
            {
                customerCars = customerCars.Where(x => x.RegistrationNumber.Contains(previewModel.RegisterNumber));
            }
            switch (previewModel.Order)
            {
                case FaultPreviewOrder.LastFault:
                    customerCars = customerCars.OrderBy(x => x.Faults.Min(y => y.IncomingDate));
                    break;
                case FaultPreviewOrder.Manufacturer:
                    customerCars = customerCars.OrderBy(x => x.Model.Manufacturer.Name);
                    break;
                case FaultPreviewOrder.Model:
                    customerCars = customerCars.OrderBy(x => x.Model.ModelName);
                    break;
                case FaultPreviewOrder.RegisterNumber:
                    customerCars = customerCars.OrderBy(x => x.RegistrationNumber);
                    break;
                default:
                    break;
            }
            var cars =
                customerCars.Select(
                    x => new RepairStatusViewModel() {Car = x, Faults = x.Faults.OrderByDescending(y => y.IncomingDate)})
                    .ToList();

            return cars;
        }


        protected override void Dispose(bool disposing)
        {
            if(disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ShowInvoide(long id)
        {
            var invoice = InvoiceFactory.GetInvoiceForFaultId(id);
            return View("Invoice", invoice);
        }
    }
}