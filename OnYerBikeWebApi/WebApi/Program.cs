using DAL.Context;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using Data.Entities;
using Data.Repositories.Abstract;
using Data.Repositories.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
//using HotelListing.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi;
using WebApi.Services.Abstract;
using WebApi.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<BikeShopDbContext>(options => {
	options.UseSqlServer(connectionString);
});
builder.Services.AddIdentityCore<ApiUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<BikeShopDbContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
	options.AddPolicy("AllowAll",
		b => b.AllowAnyHeader()
			.AllowAnyOrigin()
			.AllowAnyMethod());
});

builder.Services.AddAutoMapper(typeof(Mapper));
builder.Services.AddTransient<BikeShopDbContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

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

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();