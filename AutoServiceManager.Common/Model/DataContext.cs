using System.Data.Entity;
using AutoServiceManager.Common.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoServiceManager.Common.Model
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        
        public DataContext()
            : base("DefaultConnection", throwIfV1Schema:false)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<CarManufacturer> CarManufacturers { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<SubFault> SubFaults { get; set; }
        public DbSet<CataloguePart> CatalogueParts { get; set; }
        public DbSet<Part> UsedParts{ get; set; }
        public DbSet<WorkTime> WorkedTime{ get; set; }
        public DbSet<PartOperation> PartOperations{ get; set; }
        public DbSet<City> Cities{ get; set; }
        public DbSet<Country> Countries{ get; set; }
 
    }
}