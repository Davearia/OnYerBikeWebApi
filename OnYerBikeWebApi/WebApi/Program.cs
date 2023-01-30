using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDiDependencies(builder);
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.AddDbContext(builder);

var app = builder.Build();

builder.Services.RunDbMigrations(app);
builder.Services.SeedDb(app);
builder.Services.AddSwagger(app);
builder.Services.ApplyCorsPolicies(app);

app.UseAuthorization();
app.MapControllers();
app.Run();
