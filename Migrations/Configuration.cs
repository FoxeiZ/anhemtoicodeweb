namespace anhemtoicodeweb.Migrations
{
    using anhemtoicodeweb.Models;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<anhemtoicodeweb.Models.Model1>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "anhemtoicodeweb.Models.Model1";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(anhemtoicodeweb.Models.Model1 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Products.ForEach(p =>
            {
                p.FinalPrice = p.Price + (p.Price * p.Tax) - (p.Price * p.Discount);
            });
        }
    }
}
