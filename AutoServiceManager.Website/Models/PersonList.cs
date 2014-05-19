using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Models
{
    public class PersonList
    {
        private static DataContext db = new DataContext();

        public List<SelectListItem> temp { get; set; }

        public static PersonList GetPersonsList()
        {
            PersonList model = new PersonList();

            var customers = db.Customers.OrderBy(x => x.ID);
            model.temp = customers.Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName.ToString() }).ToList();
            model.temp.Add(new SelectListItem() { Value = "", Text = "" });

            return model;
        }
    }
}