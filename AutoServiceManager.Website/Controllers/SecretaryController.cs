using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using AutoServiceManager.Website.Models;


namespace AutoServiceManager.Website.Controllers
{
    [AuthorizeRole(ApplicationRoles.Secretary)]
    public class SecretaryController : Controller
    {
        private DataContext db = new DataContext();

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public SecretaryController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }


        public SecretaryController()
           :this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new DataContext())))
        {
             
        }

        //
        // GET: /Secretary/
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Secretary/Allcars/
        [HttpPost]
        public ActionResult Allcars(CarList listWithQuery)
        {
            return View(listWithQuery.GetCarList());
        }

        // GET: /Secretary/Allcars/
        [HttpGet]
        public ActionResult Allcars()
        {
            return View(CarList.GetAllCarList());
        }


        // GET: /Secretary/Faults/
        [HttpGet]
        public ActionResult Faults()
        {
            return View(FaultList.GetAllFaultList());
        }
        // POST: /Secretary/Faults/
        [HttpPost]
        public ActionResult Faults(FaultList ListOfFaults)
        {
            return View(FaultList.GetAllFaultList());
        }


        // POST: /Secretary/AddPerson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPerson(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var db = new DataContext();
                    model.PersonData.RegistrationTime = DateTime.Now;
                    model.PersonData.UserID = user.Id;
                    db.People.Add(model.PersonData);
                    await db.SaveChangesAsync();
                    await UserManager.AddToRoleAsync(user.Id, ApplicationRoles.Customer.ToString());

                    return Json(new { Succeeded = true, UserName = model.UserName, UserId = model.PersonData.ID});
                }
                else
                {
                    return Json(result);
                }
            }
            else {

                List<string> ListOfErrors = new List<string>();
                foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Value.Errors).ToList())
                {
                    ListOfErrors.Add(error.First().ErrorMessage.ToString());
                }
                
                return Json(new {Errors = ListOfErrors.ToArray()});
            }
          
            //return Json(model);
        }

        
        // POST: /Secretary/AddCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCar(Car car)
        {
            if (ModelState.IsValid)
            {
                var db = new DataContext();
                db.Cars.Add(car);
                await db.SaveChangesAsync();
            }
            else
            {

                List<string> ListOfErrors = new List<string>();
                foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Value.Errors).ToList())
                {
                    ListOfErrors.Add(error.First().ErrorMessage.ToString());
                }

                return Json(new { Errors = ListOfErrors.ToArray() });
            }
            return Json(new { Succeeded = true});
        }

	}
}