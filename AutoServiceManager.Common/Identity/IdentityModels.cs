using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoServiceManager.Common.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoServiceManager.Common.Identity
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

    }


    public enum ApplicationRoles
    {
        Customer,
        Secretary,
        Admin,
        Boss,
        Worker,
        Storekeeper,
        Foreman,
    }
}