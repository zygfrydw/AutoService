using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace AutoServiceManager.Common.Model
{
    public class CarManufacturer
    {
        public CarManufacturer()
        {
            
        }

        public CarManufacturer(string name)
        {
            Name = name;
        }

        public long Id { get; set; }
        [Required]
        //[Index(IsUnique = true)]
        [DisplayName("Producent")]
        [MaxLength(40)]
        public string Name { get; set; }
    }

    public class CarModel
    {
        public CarModel()
        {
            
        }
        public CarModel(CarManufacturer carManufacturer, string name)
        {
            Manufacturer = carManufacturer;
            ModelName = name;
        }

        public long Id { get; set; }
        [Required]
        [DisplayName("Producent")]
        public virtual CarManufacturer Manufacturer { get; set; }
        [Required]
        [DisplayName("Marka samochodu")]
        [MaxLength(40)]
        public string ModelName { get; set; }

    }
    public class Car
    {
        public long ID { get; set; }
        public virtual CarModel Model { get; set; }
        [Required]
        [DisplayName("Model samochodu")]
        public long ModelID { get; set; }
        
        [Required]
        [DisplayName("Rok produkcji")]
        public int ProductionYear { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Numer rejestracyjny")]
        public string RegistrationNumber { get; set; }

        public long OwnerID { get; set; }
        public virtual Customer Owner { get; set; }

        public virtual ICollection<Fault> Faults { get; set; }

        protected bool Equals(Car other)
        {
            return ID == other.ID;
        }

        public Status? CarStatus
        {
            get
            {

                try
                {
                    return (from fault in Faults
                        orderby fault.IncomingDate descending
                        select fault.RepairStatus).First();
                }
                catch
                {
                    return null;
                }
            }
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Car)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
     
}