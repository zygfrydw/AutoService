using System.Linq;
using AutoServiceManager.Common.Identity;
using AutoServiceManager.Common.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoServiceManager.Common.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoServiceManager.Common.Model.DataContext>
    {


        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AutoServiceManager.Common.Model.DataContext";
        }

        protected override void Seed(AutoServiceManager.Common.Model.DataContext context)
        {
            var initializer = new DatabaseInitializer();
            initializer.Seed(context);
            base.Seed(context);
        }
    }
}
     