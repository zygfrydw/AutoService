using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceManager.Common.DataAnnotations
{

    public class ColorAttribute : Attribute
    {
        private readonly string color;

        public ColorAttribute(string color)
        {
            this.color = color;
        }

        public string Color
        {
            get { return color; }
        }
    }
}
