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
using System.Text;
using WebApi.Services.Abstract;
using WebApi.Services.Concrete;

namespace WebApi
{

    /// <summary>
    /// Helperclass to extend IServiceCollection to make Program.cs less cluttered
    /// </summary>
    public static class ServiceExtensons
	{

		public static void AddDbContext(this IServiceCollection sv, WebApplicationBuilder builder)
		{
			var connectionString = builder.Configuration.GetConnectionString("AppDb");
			builder.Services.AddDbContext<BikeShopDbContext>(options => options.UseSqlServer(connectionString));
		}

		public static void ConfigureIdentity(this IServiceCollection services, WebApplicationBuilder builder)
		{
			builder.Services.AddIdentityCore<ApiUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<BikeShopDbContext>();
		}

		public static void ConfigureJwt(this IServiceCollection services, WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => {
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:ValidAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
			});
		}

		public static void AddDiDependencies(this IServiceCollection services, WebApplicationBuilder builder)
		{
			builder.Services.AddAutoMapper(typeof(Mapper));
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
					Name = "Authorization",
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
							Scheme = "OAuth2",
							Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
						new List<string>()
					}
				});

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "On Yer Bike!", Version = "v1" });
            });
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