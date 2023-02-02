
using DAL.Context;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAL.Seeding
{
    public class DataInitialiser
    {
              
        public static void SeedData(BikeShopDbContext context, string rootPath)
        {
            SeedProductData(context, rootPath);
            SeedProductCategoryData(context, rootPath);
            SeedProductReviewsData(context, rootPath);
            SeedProductSubcategories(context, rootPath);
            SeedUsers(context, rootPath);
            SeedAspNetUsers(context, rootPath);
            AddRolesToAdminUser(context, rootPath);
        }

        private static void SeedProductData(BikeShopDbContext context, string rootPath)
        {
            if (!context.Products.Any())
            {
                var filePath = rootPath + "Products.json";
                var productsJson = File.ReadAllText(filePath);

                if (productsJson != null)
                {
                    var products = JsonConvert.DeserializeObject<List<Product>>(productsJson);

                    if (products != null)
                    {
                        context.Products.AddRange(products);
                    }
                }

                context.SaveChanges();
            }
        }

        private static void SeedProductCategoryData(BikeShopDbContext context, string rootPath)
        {
            if (!context.ProductCategories.Any())
            {
                var filePath = rootPath + "ProductCategories.json";
                var productCategoriesJson = File.ReadAllText(filePath);

                if (productCategoriesJson != null)
                {
                    var productCategories = JsonConvert.DeserializeObject<List<ProductCategory>>(productCategoriesJson);

                    if (productCategories != null)
                    {
                        context.ProductCategories.AddRange(productCategories);
                    }
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductCategories ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductCategories OFF;");
                    transaction.Commit();
                }
            }
        }

        private static void SeedProductReviewsData(BikeShopDbContext context, string rootPath)
        {
            if (!context.ProductReviews.Any())
            {
                var filePath = rootPath + "ProductReviews.json";
                var productReviewsJson = File.ReadAllText(filePath);

                if (productReviewsJson != null)
                {
                    var productReviews = JsonConvert.DeserializeObject<List<ProductReview>>(productReviewsJson);

                    if (productReviews != null)
                    {
                        context.ProductReviews.AddRange(productReviews);
                    }
                }

                context.SaveChanges();
            }
        }

        private static void SeedProductSubcategories(BikeShopDbContext context, string rootPath)
        {
            if (!context.ProductSubcategories.Any())
            {
                var filePath = rootPath + "ProductSubcategories.json";
                var productSubcategoriesJson = File.ReadAllText(filePath);

                if (productSubcategoriesJson != null)
                {
                    var productSubcategories = JsonConvert.DeserializeObject<List<ProductSubcategory>>(productSubcategoriesJson);

                    if (productSubcategories != null)
                    {
                        context.ProductSubcategories.AddRange(productSubcategories);
                    }
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductSubcategories ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductSubcategories OFF;");
                    transaction.Commit();
                }
            }
        }

        private static void SeedUsers(BikeShopDbContext context, string rootPath)
        {
            if (!context.Users.Any())
            {
                var filePath = rootPath + "Users.json";
                var usersJson = File.ReadAllText(filePath);

                if (usersJson != null)
                {
                    var users = JsonConvert.DeserializeObject<List<User>>(usersJson);

                    if (users != null)
                    {
                        context.Users.AddRange(users);
                    }
                }

                context.SaveChanges();
            }
        }

        private static void SeedAspNetUsers(BikeShopDbContext context, string rootPath)
        {
            if (!context.ApiUsers.Any())
            {
                var filePath = rootPath + "AspNetUsers.json";
                var usersJson = File.ReadAllText(filePath);

                if (usersJson != null)
                {
                    var users = JsonConvert.DeserializeObject<List<ApiUser>>(usersJson);

                    if (users != null)
                    {
                        context.ApiUsers.AddRange(users);
                    }
                }

                context.SaveChanges();
            }
        }

        private static void AddRolesToAdminUser(BikeShopDbContext context, string rootPath)
        {
            var filePath = rootPath + "AspNetUsers.json";
            var usersJson = File.ReadAllText(filePath);

            if (usersJson != null)
            {
                var users = JsonConvert.DeserializeObject<List<ApiUser>>(usersJson);

                if(users != null){
                    var adminUser = users.Where(u => u.UserName == "AdminUser").FirstOrDefault();
                    var role = context.Roles.Where(r => r.Name == "Administrator").FirstOrDefault();

                    if (adminUser != null && role != null)
                    {                        
                        var userRole = new IdentityUserRole<string>
                        {
                            RoleId= role.Id,                            
                            UserId= adminUser.Id
                        };

                        if(!context.UserRoles.Any(ur => ur.RoleId == role.Id && ur.UserId == adminUser.Id))
                        {
                            context.UserRoles.Add(userRole);

                            context.SaveChanges();
                        }                                               
                    }
                }                
            }
        }

    }
}
