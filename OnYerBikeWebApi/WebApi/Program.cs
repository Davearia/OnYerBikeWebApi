using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDiDependencies(builder);
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddDbContext(builder);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerAuth();

var app = builder.Build();

builder.Services.UseSwagger(app);
builder.Services.RunDbMigrations(app);
builder.Services.SeedDb(app);
builder.Services.ApplyCorsPolicies(app);

app.UseAuthorization();
app.MapControllers();
app.Run();
