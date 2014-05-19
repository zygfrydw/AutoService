using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoServiceManager.Common.Identity;

namespace AutoServiceManager.Website.Models
{
    public class AuthorizeRoleAttribute :AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params ApplicationRoles[] UserRoles)
        {
            var roles = UserRoles.Aggregate("", (current, role) => current + ("," + role.ToString()));
            Roles = roles;
        }
    }
}