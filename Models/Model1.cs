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

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderPro> OrderProes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>()
            //    .Property(e => e.CategoryId)
            //    .IsFixedLength();

            modelBuilder.Entity<OrderPro>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.OrderPro)
                .HasForeignKey(e => e.IDOrder);

            //modelBuilder.Entity<Product>()
            //    .Property(e => e.CategoryId)
            //    .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.IDProduct);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrderProes)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.IDCus);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Seller)
                .HasForeignKey(e => e.SellerId);

            modelBuilder.Entity<Discount>()
                .Property(e => e.Description).IsOptional();
            modelBuilder.Entity<Discount>()
                .Property(e => e.NameDiscount).IsOptional();
        }
    }
}
