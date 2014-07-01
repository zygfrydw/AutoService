using System;
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



    public class WorkerWithSalary
    {
        public Worker Worker { get; set; }
        public decimal Salary { get; set; }
        public double WorkTime { get; set; }
    }

    public class WorkerList
    {
        private static DataContext db = new DataContext();
        public int month;
        public int year;

        public IEnumerable<WorkerWithSalary> list { get; set; }
        public string query { get; set; }
        public string order { get; set; }
        

        public string highlightDifferentUser { get; set; }

        public WorkerList()
        {

        }

        public WorkerList GetUserList(string query = null)
        {
            
            var Found = db.Workers.Include("Address").Include("Address.City").Include("Address.Country").AsQueryable();


            if (!string.IsNullOrEmpty(this.query))
            {
                Found = (from c in db.Workers
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

            var workerList = (IEnumerable<AutoServiceManager.Common.Model.Worker>) Found.ToList();
            
            //list = workerList.Select(worker =>
            //{
            //    decimal salary  = db.WorkedTime.Where(x=> x.WorkerId == worker.ID).Sum(time => time.WorkCost);
            //    double workTime  = db.WorkedTime.Where(x=> x.WorkerId == worker.ID).Sum(time => time.WorkDuration);
            //    return new WorkerWithSalary() {Worker = worker, Salary = salary, WorkTime = workTime};
            //}).ToList();
            var workesWithSalaryList = new List<WorkerWithSalary>();
            foreach (var worker in workerList)
            {
                var wws = new WorkerWithSalary();
                wws.Worker = worker;
                var tmp = db.WorkedTime.Where(x => x.WorkerId == worker.ID).ToList();
                var workTimes = db.WorkedTime.Where(x => x.WorkerId == worker.ID && x.EndTime.Month == month && x.EndTime.Year == year).ToList();
                wws.Salary = workTimes.Sum(time => time.WorkCost);
                wws.WorkTime = workTimes.Sum(time => time.WorkDuration);
                workesWithSalaryList.Add(wws);
            }
            list = workesWithSalaryList;

            return this;
        }
    }
}