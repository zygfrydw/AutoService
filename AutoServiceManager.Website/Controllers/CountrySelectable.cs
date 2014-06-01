using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Controllers
{
    public class CountrySelectable
    {
        public static IEnumerable<SelectListItem> GetSelectList()
        {
            using (var db = new DataContext())
            {
                var coutries = db.Countries.Select(c => new SelectListItem() {Text = c.Name, Value = c.Id.ToString()}).ToList();
                coutries.Find(x => x.Text == "Polska").Selected = true;
                return coutries;
            }
        }
    }
}