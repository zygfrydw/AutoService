using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.DataAnnotations;

namespace AutoServiceManager.Website.Models
{
    public static class MvcHtmlHelpers
    {
        public static MvcHtmlString DescriptionFor<TModel, Enum>(this HtmlHelper<TModel> self, Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return MvcHtmlString.Create(attributes[0].Description);
            else
                return MvcHtmlString.Create(value.ToString());
        }
        public static MvcHtmlString ColorFor<TModel, Enum>(this HtmlHelper<TModel> self, Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ColorAttribute[] attributes = (ColorAttribute[])fi.GetCustomAttributes(typeof(ColorAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return MvcHtmlString.Create(attributes[0].Color);
            else
                return MvcHtmlString.Create(value.ToString());
        }

        public static MvcHtmlString ClassFor<TModel, Enum>(this HtmlHelper<TModel> self, Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (HtmlClassAttribute[])fi.GetCustomAttributes(typeof(HtmlClassAttribute), false);

            return MvcHtmlString.Create(attributes.Length > 0 ? attributes[0].ClassName : value.ToString());
        }
    }
}