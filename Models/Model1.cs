using anhemtoicodeweb.Migrations;
using System.Data.Entity;

namespace anhemtoicodeweb.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Database")
        {
            Database.SetInitializer
         (new MigrateDatabaseToLatestVersion<Model1, Configuration>());
        }

        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderPro> OrderProes { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>()
                .Property(e => e.IDCus);

            modelBuilder.Entity<Category>()
                .Property(e => e.IDCate)
                .IsFixedLength();

            modelBuilder.Entity<OrderPro>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.OrderPro)
                .HasForeignKey(e => e.IDOrder);

            modelBuilder.Entity<Product>()
                .Property(e => e.IDCate)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.IDProduct);
        }
    }
}
