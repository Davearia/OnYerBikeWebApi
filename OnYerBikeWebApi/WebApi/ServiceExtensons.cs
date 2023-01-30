using DAL.Context;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using DAL.Seeding;
using Data.Entities;
using Data.Repositories.Abstract;
using Data.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{

    /// <summary>
    /// Class to make Program.cs less cluttered
    /// </summary>
    public static class ServiceExtensons
    {

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<BikeShopDbContext>().AddDefaultTokenProviders();
        }

        public static void AddDiDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<BikeShopDbContext>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
        }

        public static void ApplyCorsPolicies(this IServiceCollection services, WebApplication app)
        {
            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        }

        public static void AddSwagger(this IServiceCollection services, WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public static void AddDbContext(this IServiceCollection sv, WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("AppDb");
            builder.Services.AddDbContext<BikeShopDbContext>(options => options.UseSqlServer(connectionString));

        }

        public static void RunDbMigrations(this IServiceCollection sv, WebApplication app)
        {
            //Apply any DB migrations
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<BikeShopDbContext>();
                if (context != null)
                {
                    context.Database.Migrate();
                }
            }
        }

        public static void SeedDb(this IServiceCollection sv, WebApplication app)
        {
            //Seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<BikeShopDbContext>();

                if (context != null)
                {
                    var rootPath = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf("\\"))
                        + app.Configuration.GetSection("AppSettings:jsonPath").Value;

                    DataInitialiser.SeedData(context, rootPath);
                }
            }
        }

    }
}