using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoServiceManager.Website.Models
{
    public class TimeHelper
    {
        public static SelectList GetMonthsList(int month)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem(){Text = "Styczeń", Value = "1", Selected = month == 1});
            items.Add(new SelectListItem() { Text = "Luty", Value = "2", Selected = month == 2 });
            items.Add(new SelectListItem() { Text = "Marzec", Value = "3", Selected = month == 3 });
            items.Add(new SelectListItem() { Text = "Kwiecień", Value = "4", Selected = month == 4 });
            items.Add(new SelectListItem() { Text = "Maj", Value = "5", Selected = month == 5 });
            items.Add(new SelectListItem() { Text = "Czerwiec", Value = "6", Selected = month == 6 });
            items.Add(new SelectListItem() { Text = "Lipiec", Value = "7", Selected = month == 7 });
            items.Add(new SelectListItem() { Text = "Sierpień", Value = "8", Selected = month == 8 });
            items.Add(new SelectListItem() { Text = "Wrzesień", Value = "9", Selected = month == 9 });
            items.Add(new SelectListItem(){Text = "Październik", Value = "10", Selected = month == 10});
            items.Add(new SelectListItem() { Text = "Listopad", Value = "11", Selected = month == 11 });
            items.Add(new SelectListItem() { Text = "Grudzień", Value = "12", Selected = month == 12 });
            return new SelectList(items, "Value", "Text");
        }
    }
}