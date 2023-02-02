using DAL.Context;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using DAL.Seeding;
using Data.Entities;
using Data.Repositories.Abstract;
using Data.Repositories.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Services.Abstract;
using WebApi.Services.Concrete;

namespace WebApi
{

    /// <summary>
    /// Helperclass to extend IServiceCollection to make Program.cs less cluttered
    /// </summary>
    public static class ServiceExtensons
	{

		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);

			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
			builder.AddEntityFrameworkStores<BikeShopDbContext>().AddDefaultTokenProviders();
		}

		public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
		{
			var issuer = configuration.GetSection("Jwt:Issuer").Value;

			//This value is in appsetting for our demo app, it would be in something more secure for prod like environment variables etc!!!!
			//https://www.youtube.com/watch?v=iIsaEzNXhoo 16 minutes in
			var key = configuration.GetSection("Jwt:Key").Value ?? string.Empty;

			services.AddAuthentication(o =>
			{
				o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o =>
			{
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = issuer,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key))
				};
			});
		}

		public static void AddDiDependencies(this IServiceCollection services, WebApplicationBuilder builder)
		{
			builder.Services.AddTransient<BikeShopDbContext>();
			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IAuthManager, AuthManager>();           

            builder.Services.AddTransient<IProductRepository, ProductRepository>();
			builder.Services.AddTransient<IOrderRepository, OrderRepository>();
			builder.Services.AddTransient<IUserRepository, UserRepository>();            
        }

		public static void ApplyCorsPolicies(this IServiceCollection services, WebApplication app)
		{			
			app.UseCors(builder =>
			{
				builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
			});
		}

		public static void UseSwagger(this IServiceCollection services, WebApplication app)
		{			
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}			
		}

        public static void AddSwaggerAuth(this IServiceCollection services)
        {           
            services.AddSwaggerGen(c =>
            {
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"JWT Authorisation header using the Bearer scheme.
						Enter 'Bearer' [space] and then your token in the text input below.
						Example Bearer 12345abcdef",
					Name = "Authorisation",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "0auth2",
							Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
						new List<string>()
					}
				});

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "On Yer Bike!", Version = "v1" });
            });
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