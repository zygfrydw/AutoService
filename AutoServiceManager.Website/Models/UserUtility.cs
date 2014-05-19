using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using System.Web;
using AutoServiceManager.Common.Model;
using Microsoft.AspNet.Identity;

namespace AutoServiceManager.Website.Models
{
    public static class UserUtility
    {
        

        public static Customer GetCustomer(this IPrincipal user, DataContext db)
        {
            if (user == null)
                return null;
            if (db == null)
                return null;
            var userId = user.Identity.GetUserId();
            var customer = (from person in db.Customers
                            where person.UserID == userId
                            select person).FirstOrDefault();
            return customer;
        }
        public static Worker GetWorker(this IPrincipal user, DataContext db)
        {
            if (user == null)
                return null;
            if (db == null)
                return null;
            var userId = user.Identity.GetUserId();
            var customer = (from person in db.Workers
                            where person.UserID == userId
                            select person).FirstOrDefault();
            return customer;
        }
    }
}