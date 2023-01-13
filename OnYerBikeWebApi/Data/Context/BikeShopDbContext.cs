using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class BikeShopDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<User> Users { get; set; }

        public BikeShopDbContext(DbContextOptions<BikeShopDbContext> options) : base(options)
        {
        }

        public BikeShopDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Deal with decimal column issue: https://stackoverflow.com/questions/62550667/validation-30000-no-type-specified-for-the-decimal-column
            var decimalProps = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }                      
        }        

    }
}
