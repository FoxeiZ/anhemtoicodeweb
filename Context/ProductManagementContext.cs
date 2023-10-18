using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using anhemtoicodeweb.Models;

namespace anhemtoicodeweb.Context
{
    public class ProductManagementContext : DbContext
    {
        public ProductManagementContext() : base("ProductManagementContextDB")
        {
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}