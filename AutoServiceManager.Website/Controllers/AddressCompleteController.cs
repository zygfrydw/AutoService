using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Controllers
{
    public class AddressCompleteController : Controller
    {
        private DataContext db = new DataContext();
        // GET: AddressComplete
        public JsonResult GetCities(string term)
        {
            var cities = db.Cities.Where(x=> x.Name.ToLower().Contains(term.ToLower()))
                .Select(x => x.Name).Distinct().ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}