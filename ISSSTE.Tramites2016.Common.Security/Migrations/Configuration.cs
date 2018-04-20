using ISSSTE.Tramites2016.Common.Security.Identity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ISSSTE.Tramites2016.Common.Security.Migrations
{    
    public sealed class Configuration : DbMigrationsConfiguration<IsssteIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IsssteIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
