using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoServiceManager.Common.Identity;

namespace AutoServiceManager.Website.Models
{
    public static class MenuSecurity
    {
        public static bool IsAdmin
        {
            get { //return HttpContext.Current.User.IsInRole(ApplicationRoles.Admin.ToString()); }

                return true;
                }
            }
    }
}