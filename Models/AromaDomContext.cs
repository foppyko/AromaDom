using System.Collections.Generic;
using System.Data.Entity;

namespace AromaDom.Web.Models
{
    public class AromaDomContext : DbContext
    {
        public AromaDomContext() : base("name=AromaDomConnection")
        {
        }

        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<MaterialProduct> MaterialProducts { get; set; }
    }
}