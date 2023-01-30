using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Resolve any dependency injected instances
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
