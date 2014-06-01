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

        public string highlightDifferentUser { get; set; }

        public UserList GetUserList(string query = null)
        {
             
            this.query=(query!=null)?query:this.query;
            var Found = (from c in db.People
                               select c).AsQueryable(); ;
            if (!string.IsNullOrEmpty(this.query))
            {
                Found = (from c in db.People
                        where
                        c.FirstName.Contains(this.query) ||
                        c.SecondName.Contains(this.query) ||
                        c.Address.City.Contains(this.query) ||
                        c.Address.Country.Contains(this.query) ||
                        c.Address.Email.Contains(this.query) ||
                        c.Address.PhoneNumber.Contains(this.query) ||
                        c.Address.Street.Contains(this.query) ||
                        c.Address.ZipCode.Contains(this.query) 
                        select c).AsQueryable();
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
                Found = Found.OrderBy(c => c.Address.City);
            }
            else if (this.order == "AddressCountry")
            {
                Found = Found.OrderBy(c => c.Address.Country);
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

            list = (IEnumerable<AutoServiceManager.Common.Model.Person>)Found.ToList();

            return this;
        }
    }
}