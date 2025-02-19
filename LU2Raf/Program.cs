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

app.MapGet("/", () =>
{
    var asciiArt = @"
 ________  ________  ___          ___  ________   ________ ________  ________  _____ ______   ________  _________  ___  ________  ________      
|\   __  \|\   __  \|\  \        |\  \|\   ___  \|\  _____\\   __  \|\   __  \|\   _ \  _   \|\   __  \|\___   ___\\  \|\   __  \|\   ___  \    
\ \  \|\  \ \  \|\  \ \  \       \ \  \ \  \\ \  \ \  \__/\ \  \|\  \ \  \|\  \ \  \\\__\ \  \ \  \|\  \|___ \  \_\ \  \ \  \|\  \ \  \\ \  \   
 \ \   __  \ \   ____\ \  \       \ \  \ \  \\ \  \ \   __\\ \  \\\  \ \   _  _\ \  \\|__| \  \ \   __  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \  
  \ \  \ \  \ \  \___|\ \  \       \ \  \ \  \\ \  \ \  \_| \ \  \\\  \ \  \\  \\ \  \    \ \  \ \  \ \  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ 
   \ \__\ \__\ \__\    \ \__\       \ \__\ \__\\ \__\ \__\   \ \_______\ \__\\ _\\ \__\    \ \__\ \__\ \__\   \ \__\ \ \__\ \_______\ \__\\ \__\
    \|__|\|__|\|__|     \|__|        \|__|\|__| \|__|\|__|    \|_______|\|__|\|__|\|__|     \|__|\|__|\|__|    \|__|  \|__|\|_______|\|__| \|__|";

    var result = $"{asciiArt}\n\n\n\nStatus:\nThe API is up.\n\nConnection string found:\n{(sqlConnectionStringFound ? "Yes." : "WARNING! No.")}";

    return result;
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
