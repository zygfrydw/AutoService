using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoServiceManager.Website.Controllers
{
    public class UserAccountsController : Controller
    {
        //
        // GET: /UserAccounts/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /UserAccounts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserAccounts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserAccounts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserAccounts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserAccounts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserAccounts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
