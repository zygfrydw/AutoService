using System;

namespace AutoServiceManager.Website.Models
{
    public class HtmlClassAttribute : Attribute
    {
        public string ClassName { get; set; }

        public HtmlClassAttribute(string className)
        {
            ClassName = className;
        }
    }
}