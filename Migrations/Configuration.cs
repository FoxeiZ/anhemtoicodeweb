namespace anhemtoicodeweb.Migrations
{
    using Microsoft.Ajax.Utilities;
    using System.Data.Entity.Migrations;

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
            context.OrderProes.ForEach(p =>
            {
                p.State = "Đang giao hàng";
            });
        }
    }
}
