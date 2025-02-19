using LU2Raf.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
builder.Services.AddTransient<IEnvironment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<IObject2DRepository>(o => new Object2DRepository(sqlConnectionString));

var app = builder.Build();

// ASCII art bovenaan
Console.WriteLine(@"
 ________  ________  ___
|\   __  \|\   __  \|\  \
\ \  \|\  \ \  \|\  \ \  \
 \ \   __  \ \   ____\ \  \
  \ \  \ \  \ \  \___|\ \  \
   \ \__\ \__\ \__\    \ \__\
    \|__|\|__|\|__|     \|__|

 ___  ________   ________ ________  ________  _____ ______   ________  _________  ___  ________  ________      
|\  \|\   ___  \|\  _____\\   __  \|\   __  \|\   _ \  _   \|\   __  \|\___   ___\\  \|\   __  \|\   ___  \    
\ \  \ \  \\ \  \ \  \__/\ \  \|\  \ \  \|\  \ \  \\\__\ \  \ \  \|\  \|___ \  \_\ \  \ \  \|\  \ \  \\ \  \   
 \ \  \ \  \\ \  \ \   __\\ \  \\\  \ \   _  _\ \  \\|__| \  \ \   __  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \  
  \ \  \ \  \\ \  \ \  \_| \ \  \\\  \ \  \\  \\ \  \    \ \  \ \  \ \  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ 
   \ \__\ \__\\ \__\ \__\   \ \_______\ \__\\ _\\ \__\    \ \__\ \__\ \__\   \ \__\ \ \__\ \_______\ \__\\ \__\
    \|__|\|__| \|__|\|__|    \|_______|\|__|\|__|\|__|     \|__|\|__|\|__|    \|__|  \|__|\|_______|\|__| \|__|
");

// De oorspronkelijke tekst
app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
