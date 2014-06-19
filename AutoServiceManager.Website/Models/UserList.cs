using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Linq.Dynamic;
using AutoServiceManager.Common.Model;
namespace AutoServiceManager.Website.Models
{
    public class UserList
    {
        private static DataContext db = new DataContext();

        public IEnumerable<Person> list { get; set; }
        public string query { get; set; }
        public string order { get; set; }
        public string typeFilter { get; set; }

        public string NotActiveFilter { get; set; }
        public string BlockedFilter { get; set; }

        public string highlightDifferentUser { get; set; }

        public UserList GetUserList(string query = null)
        {
            db = new DataContext();
            var Found = db.People.Include("Address").Include("Address.City").Include("Address.Country").AsQueryable();
            
            if (!string.IsNullOrEmpty(this.typeFilter) && !this.typeFilter.Equals(""))
            {
                if (this.typeFilter.Equals("Customer"))
                {
                    Found = Found.OfType<Customer>();
                }
                else if (this.typeFilter.Equals("Worker"))
                {
                    Found = Found.OfType<Worker>();
                }
            }

            if (!string.IsNullOrEmpty(this.query))
            {
                Found = (from c in db.People
                        where
                        c.FirstName.Contains(this.query) ||
                        c.SecondName.Contains(this.query) ||
                        c.Address.City.Name.Contains(this.query) ||
                        c.Address.Country.Name.Contains(this.query) ||
                        c.Address.Email.Contains(this.query) ||
                        c.Address.PhoneNumber.Contains(this.query) ||
                        c.Address.Street.Contains(this.query) ||
                        c.Address.ZipCode.Contains(this.query) 
                        select c).AsQueryable();
            }

            if (!string.IsNullOrEmpty(this.NotActiveFilter))
            {
                if (this.NotActiveFilter == "NOT")
                {
                    Found = Found.Where(c => c.NotActive == 0);
                }
                else if (this.NotActiveFilter == "ONLY")
                {
                    Found = Found.Where(c => c.NotActive == 1);
                }
            }
            if (!string.IsNullOrEmpty(this.BlockedFilter))
            {
                if (this.BlockedFilter == "NOT")
                {
                    Found = Found.Where(c => c.Blocked == 0);
                }
                else if (this.BlockedFilter == "ONLY")
                {
                    Found = Found.Where(c => c.Blocked == 1);
                }
            }

            else if (this.order == "FirstName")
            {
                Found = Found.OrderBy(c => c.FirstName);
            }
            else if (this.order == "SecondName")
            {
                Found = Found.OrderBy(c => c.SecondName);
            }

            else if (this.order == "AddressCity")
            {
                Found = Found.OrderBy(c => c.Address.City.Name);
            }

            else if (this.order == "AddressCountry")
            {
                Found = Found.OrderBy(c => c.Address.Country.Name);
            }

            else if (this.order == "AddressEmail")
            {
                Found = Found.OrderBy(c => c.Address.Email);
            }
            else if (this.order == "AddressPhoneNumber")
            {
                Found = Found.OrderBy(c => c.Address.PhoneNumber);
            }
            else if (this.order == "AddressStreet")
            {
                Found = Found.OrderBy(c => c.Address.Street);
            }
            else if (this.order == "AddressZipCode")
            {
                Found = Found.OrderBy(c => c.Address.ZipCode);
            }

            list = Found.ToList();

            return this;
        }
    }
}