using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Common.Invoice
{
    public class InvoiceFactory
    {
        public static Invoice GetInvoiceForFaultId(long id)
        {
            try
            {
                var invoice = new Invoice();
                using (var db = new DataContext())
                {
                    var fault = db.Faults.Include(f => f.RelatedCar).Include(f => f.RelatedCar.Model.Manufacturer).First(x => x.ID == id);
                    invoice.FaultId = id;
                    invoice.Car = fault.RelatedCar;
                    invoice.Customer = invoice.Car.Owner;
                    var adress = invoice.Customer.Address;
                    invoice.City = db.Cities.Find(adress.CityId).Name;
                    invoice.Country = db.Countries.Find(adress.CountryId).Name;
                    var subFaults = db.SubFaults.Include(sub => sub.WorkersHours).Include(sub => sub.UsedParts.Select(y => y.PartFromCatalogue)).Where(x => x.FaultId == fault.ID);
                    var parts = subFaults.SelectMany(subfault => subfault.UsedParts).ToList();
                    parts.ForEach( x=> x.PartFromCatalogue = db.CatalogueParts.Find(x.CataloguePartId)); //Todo refactor this!!!!
                    invoice.Parts = parts;
                    invoice.PartCosts = parts.Sum(part => part.Cost);
                    invoice.ExtraCosts = 0;
                    var workTimes = subFaults.SelectMany(subFault => subFault.WorkersHours).ToArray();
                    invoice.WorkedHours = workTimes.Sum(time => time.WorkDuration);
                    invoice.WorkCosts = workTimes.Sum(time => time.WorkCost);
                    return invoice;
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Can't create invoice fror this fault");
            }
        }
    }
}