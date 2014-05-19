using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoServiceManager.Common.Model
{
    internal class DatabaseInitializer 
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public void Seed(DataContext db)
        {
            AddCarDatabase(db);
            AddAplicationRolesAndUsers(db);
        }
         private void AddCarDatabase(DataContext context)
        {
            var carManufacturer = new CarManufacturer("Volkswagen");
            carManufacturer = context.AddIfDistinct(carManufacturer);
            var carModel = new CarModel(carManufacturer, "Polo");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "Golf");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "Jetta");
            context.AddIfDistinct(carModel);

            ////---------------------------------------------------------
            carManufacturer = new CarManufacturer("Audi");
            carManufacturer = context.AddIfDistinct(carManufacturer);
            carModel = new CarModel(carManufacturer, "A1");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "A2");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "A3");
            context.AddIfDistinct(carModel);
            
            carManufacturer = new CarManufacturer("Renault");
            carManufacturer = context.AddIfDistinct(carManufacturer);
            carModel = new CarModel(carManufacturer, "Clio");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "Koleos");
            context.AddIfDistinct(carModel);
            carModel = new CarModel(carManufacturer, "Megane");
            context.AddIfDistinct(carModel);
        }

        

        private void AddAplicationRolesAndUsers(DataContext context)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            AddAplicationRoles();
            AddUser("Admin", "Admin123", ApplicationRoles.Admin);
            AddUser("Secretary", "Secretary123", ApplicationRoles.Secretary);
            AddUser("Storekeeper", "Storekeeper123", ApplicationRoles.Storekeeper);
        }

        private void AddUser(string name, string password, ApplicationRoles role)
        {
            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = name;
            var adminresult = userManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, role.ToString());
            }
        }

        private void AddAplicationRoles()
        {
            foreach (var role in Enum.GetNames(typeof (ApplicationRoles)))
            {
                //Create Role Admin if it does not exist
                if (!roleManager.RoleExists(role))
                {
                    var roleresult = roleManager.Create(new IdentityRole(role));
                }
            }
        }
    }
}
    


static class SeedUtility
{
    public static CarManufacturer AddIfDistinct(this DataContext context, CarManufacturer carManufacturer)
    {
        var manufacturer = context.CarManufacturers.FirstOrDefault(c => c.Name == carManufacturer.Name);
        if (manufacturer == null)
            return context.CarManufacturers.Add(carManufacturer);
        return manufacturer;
    }

    public static void AddIfDistinct(this DataContext context, CarModel carModel)
    {
        if (!context.CarModels.Any(c => c.Manufacturer.Id == carModel.Manufacturer.Id && c.ModelName == carModel.ModelName ))
               context.CarModels.Add(carModel);
    }
}