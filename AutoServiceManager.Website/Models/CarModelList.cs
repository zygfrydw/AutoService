using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Models
{
    public class CarModelList
    {
        private static DataContext db = new DataContext();
        
        public IEnumerable<SelectListItem> Manufacturers { get; set; }
        public IEnumerable<SelectListItem> Models { get; set; }

        public static CarModelList GetModelList()
        {
            var modelList = new CarModelList();
            var manufacturers = db.CarManufacturers.OrderBy(x => x.Name);
            modelList.Manufacturers = manufacturers.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            var manufacturer = manufacturers.FirstOrDefault();
            modelList.Models = GetModelLsSelectListItems(manufacturer);
            return modelList;
        }
        private static IEnumerable<SelectListItem> GetModelLsSelectListItems(CarManufacturer manufacturer)
        {
            return manufacturer != null
                ? GetModelsForManufacturer(manufacturer.Id)
                : new[] { new SelectListItem() };
        }
        public static IEnumerable<SelectListItem> GetModelsForManufacturer(long id)
        {
            var items =
                db.CarModels.Where(x => x.Manufacturer.Id == id)
                    .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.ModelName });
            return items;
        }
    }
}