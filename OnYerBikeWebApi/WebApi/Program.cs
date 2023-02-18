using WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext(builder);
builder.Services.ConfigureIdentity(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDiDependencies(builder);
builder.Services.ConfigureJwt(builder);
builder.Services.AddSwaggerAuth();

var app = builder.Build();
builder.Services.RunDbMigrations(app);
builder.Services.SeedDb(app);
builder.Services.UseSwagger(app);
app.UseHttpsRedirection();
builder.Services.ApplyCorsPolicies(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();