using System;
using System.Collections.Generic;

namespace AutoServiceManager.Common.Model
{
    public class Customer : Person
    {
        public DateTime RegistrationTime { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}