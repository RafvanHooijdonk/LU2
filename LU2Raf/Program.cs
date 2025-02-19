using LU2Raf.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
builder.Services.AddTransient<IEnvironment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<IObject2DRepository>(o => new Object2DRepository(sqlConnectionString));

var app = builder.Build();
app.MapGet("/", () => "Hello world, the API is up!");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
