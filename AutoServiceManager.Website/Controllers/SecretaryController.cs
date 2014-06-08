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
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace AutoServiceManager.Website.Controllers
{
    //[AuthorizeRole(ApplicationRoles.Secretary)]
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


        // GET: /Secretary/
        [HttpPost]
        public ActionResult CreateFault(Fault obj)
        {
            if (ModelState.IsValid)
            {
                db.Faults.Add(obj);
                db.SaveChanges();
            }
            return RedirectToAction("Faults");
        }
        

        // GET: /Secretary/
        [HttpPost]
        public ActionResult DynamicSearch(String query)
        {
            CarList ListOfCars = CarList.GetAllCarList();
            List<object> list = new List<object>();

            foreach (var obj in ListOfCars.GetCarList(query).list){
                list.Add(new {
                    ID = obj.ID,
                    OwnerName = obj.Owner.FirstName,
                    OwnerSurname = obj.Owner.SecondName,
                    Model = obj.Model.ModelName,
                    Manufacturer = obj.Model.Manufacturer.Name
                });
            }
            return Json(new { cars = list });
        }

        // GET: /Secretary/
        public ActionResult Index()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ViewBag.cities = js.Serialize(db.Cities.Select(x => x.Name).ToArray());
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


        // GET: /Secretary/FaultCreate/
        [HttpGet]
        public ActionResult FaultCreate()
        {
            return View(new Fault());
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


        // GET: /Secretary/AllUsers/
        [HttpGet]
        public ActionResult AllUsers()
        {
            var temp = new UserList();
            return View(temp.GetUserList());
        }
        [HttpPost]
        public ActionResult AllUsers(UserList temp)
        {
            return View(temp.GetUserList());
        }


        // GET: /Secretary/NewUser/
        [HttpGet]
        public ActionResult Newuser()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ViewBag.cities = js.Serialize(db.Cities.Select(x => x.Name).ToArray());
            return View(new RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Newuser(RegisterViewModel model,string roles)
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
                    City city = FindOrCreateCity(model.PersonData.Address.TempCity);
                    model.PersonData.Address.City = city;
                    model.PersonData.Address.CityId = city.Id;
                    db.People.Add(model.PersonData);
                    await db.SaveChangesAsync();

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    ApplicationRoles roleOB;
                    foreach (string roleST in js.Deserialize<String[]>(roles))
                    {
                        if (ApplicationRoles.TryParse<ApplicationRoles>(roleST, out roleOB)) {
                            await UserManager.AddToRoleAsync(user.Id, roleOB.ToString());
                        }
                    }
                    return RedirectToAction("AllUsers");
                }
            }

            return View(new RegisterViewModel());
        }

        protected City FindOrCreateCity(string CityName) {
            City cit = db.Cities.FirstOrDefault(city => city.Name.Contains(CityName));
            if (cit == null)
            {
                cit = new City();
                cit.Name = CityName;
                db.Cities.Add(cit);
            }
            return cit;
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
                    model.PersonData.RegistrationTime = DateTime.Now;
                    model.PersonData.UserID = user.Id;
                    City city = FindOrCreateCity(model.PersonData.Address.TempCity);
                    model.PersonData.Address.City = city;
                    model.PersonData.Address.CityId = city.Id;
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