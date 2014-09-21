using System;
using System.Collections.Generic;

namespace AutoServiceManager.Common.Model
{
    public class Worker : Person
    {
        public DateTime HireTime { get; set; }
        public virtual ICollection<Fault> Faults { get; set; }
    }
}