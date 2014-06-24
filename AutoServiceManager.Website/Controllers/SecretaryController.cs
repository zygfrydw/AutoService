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


        // GET: /Secretary/FaultCreate/ID
        [HttpGet]
        public ActionResult FaultCreate(int? id)
        {
            ViewBag.CarFromSession = null;
            string GetCarFromSession = Request.QueryString["getCarFromSession"];
            if (GetCarFromSession != null && GetCarFromSession.Equals("true")) {
                int CarID = (int)Session["lastCarID"];
                if (CarID!= null)
                {
                    Car CarFromSession = db.Cars.Include("Model").Include("Owner").FirstOrDefault(f => f.ID == CarID);
                    ViewBag.CarFromSession = CarFromSession;        
                }
            }
                

            ViewBag.loaded = false;
            if (id != null)
            {
                Fault fault = db.Faults.Include("RelatedCar").Include("RelatedCar.Model").Include("RelatedCar.Owner").FirstOrDefault(f => f.ID == id);
                if (fault == null)
                {
                    return View(new Fault());
                }
                ViewBag.loaded = true;
                return View(fault);
            }
            else {
                return View(new Fault());
            }
        }
        // GET: /Secretary/
        [HttpPost]
        public ActionResult FaultCreate(Fault obj)
        {
            Fault f = db.Faults.Include("RelatedCar").Include("RelatedCar.Model").Include("RelatedCar.Owner").FirstOrDefault(x => x.ID == obj.ID);
            ViewBag.loaded = (f != null);
            if (ModelState.IsValid)
            {
                if (f != null)
                {
                    f.IncomeToService = obj.IncomeToService;
                    f.IncomingDate = obj.IncomingDate;
                    f.PredictedEndDate = obj.PredictedEndDate;
                    f.RealeseDate = obj.RealeseDate;
                    
                    f.RepairStatus = obj.RepairStatus;
                    f.SubFaults = obj.SubFaults;
                    
                    f.CarID = obj.CarID;
                    f.RelatedCar = db.Cars.FirstOrDefault(x => x.ID == obj.CarID);

                    f.Decription = obj.Decription;
                }
                else {
                    db.Faults.Add(obj);
                }
                db.SaveChanges();
                return RedirectToAction("Faults");
            }
            else {
                List<string> ListOfErrors = new List<string>();
                foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Value.Errors).ToList())
                {
                    ListOfErrors.Add(error.First().ErrorMessage.ToString());
                }
                ViewBag.errors = ListOfErrors;
                if (f == null)
                {
                    return View(obj);
                }
                else{
                    return View(f);
                }
                
            }
        }


        // GET: /Secretary/Faults/
        [HttpGet]
        public ActionResult Faults(long? SpecyficCar)
        {
            if (SpecyficCar == null)
            {
                return View(FaultList.GetAllFaultList());
            }
            else {
                FaultList list = new FaultList(SpecyficCar);
                return View(list.GetFaultList());
            }            
        }
        // POST: /Secretary/Faults/
        [HttpPost]
        public ActionResult Faults(FaultList ListOfFaults)
        {
            return View(ListOfFaults.GetFaultList());
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
            return View(new RegisterVieworkerModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Newuser(RegisterVieworkerModel model, string roles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var db = new DataContext();
                    model.PersonData.HireTime = DateTime.Now;
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

            return View(new RegisterVieworkerModel());
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
                Session["lastCarID"] = (int)car.ID;
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

        // POST: /Secretary/ChangeUserStatus/
        [HttpPost]
        public ActionResult ChangeUserStatus(int Id, String Operation, short Value)
        {

            Person Person = db.People.FirstOrDefault(p => p.ID == Id);

            if (Operation=="NotActive")
            {
                Person.NotActive = Value;
            }
            else if (Operation == "Blocked")
            {
                Person.Blocked = Value;
            }
            db.SaveChanges();

            
            return Json(new { Operation = Operation, Id = Id, Value = Value });
        }


        protected bool ToPrint = false;
        protected int IntId;

        // GET: /Secretary/Invoice/
        [AllowAnonymous]
        public ActionResult Invoice(int? id,int? pdf)
        {
            InvoiceView Invoice = null;

            ViewBag.id = id;
            ViewBag.pdf = pdf;

            if (id != null)
            {
                Invoice = new InvoiceView((int)id);
            }
            else {
                Invoice = new InvoiceView();
            }
            
            
            return View(Invoice);
        }


        public ActionResult PrintInvoice(int? id, int? pdf)
        {
            return new Rotativa.ActionAsPdf("Invoice", new { id = id, pdf = pdf }) { FileName = "Invoice.pdf" };
        }

        [HttpPost]
        public ActionResult AddModelOrManufacturer(String Action, String Value, String ManufacturerId)
        {
            if(Value == ""){
                return Json(db.CarManufacturers.ToList());
            }

            if (Action == "AddManufacturer")
            {
                CarManufacturer CarManufacturer = new CarManufacturer();
                CarManufacturer.Name = Value;
                db.CarManufacturers.Add(CarManufacturer);
                db.SaveChanges();
            }
            else if (Action == "AddModel" && ManufacturerId !="")
            {
                CarModel Model = new CarModel();
                int CarManufacturerID = int.Parse(ManufacturerId);
                CarManufacturer CarManufacturer = db.CarManufacturers.FirstOrDefault(f => f.Id == CarManufacturerID);
                Model.ModelName = Value;
                Model.Manufacturer = CarManufacturer;
                db.CarModels.Add(Model);
                db.SaveChanges();
            }
            
            return Json(db.CarManufacturers.ToList());
        }
        

	}
}