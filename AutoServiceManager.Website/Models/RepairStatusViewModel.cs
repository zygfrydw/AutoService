using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Models
{
    public class RepairStatusViewModel
    {
        public Common.Model.Car Car { get; set; }

        public Status Status
        {
            get
            {
                var last = Faults.FirstOrDefault();
                if (last != null) return last.RepairStatus;
                else
                {
                    return Status.Relesed;
                }
            }
        }
        public IOrderedEnumerable<Common.Model.Fault> Faults { get; set; }
    }
}