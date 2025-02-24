using LU2Raf.Repositories;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
builder.Services.AddTransient<IEnvironment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<IObject2DRepository>(o => new Object2DRepository(sqlConnectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Authorization    
builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = true;

        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
    })
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString;
    });

builder.Services
    .AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
    .Configure(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/account")
    .MapIdentityApi<IdentityUser>();

app.MapPost("/account/logout",
    async (SignInManager<IdentityUser> SignInManager,
    [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await SignInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .RequireAuthorization();

app.MapControllers()
    .RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

    var result = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>API Status</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: white;
                background-color: black;
                margin: 0;
                padding: 20px;
            }}
            pre {{
                font-size: 18px;
                white-space: pre-wrap;
            }}
            .countdown {{
                font-size: 18px;
                color: yellow;
                margin-top: 10px;
            }}
            .message {{
                font-size: 18px;
                color: yellow;
                margin-top: 20px;
            }}
        </style>
    </head>
    <body>
        <h1>API</h1>
        <pre>{asciiArt}</pre>
        <br>
        <h1>Status:<br></h1>
        <p>The API is up.</p>
        <h1>Connection string found:<br></h1> 
        <p>{(sqlConnectionStringFound ? "Yes." : "WARNING! No.")}</p>

        <div id='countdown' class='countdown'>
        <h1>Status update in:</h1>
        <span id='timer'>30</span> seconds
        </div>

        <div id='updateMessage' class='message'>
            <!-- Data update message will appear here -->
        </div>

        <script>
            let countdownTimer = 30;
            let countdownElement = document.getElementById('timer');
            let updateMessageElement = document.getElementById('updateMessage');

            function updateCountdown() {{
                countdownTimer--;
                countdownElement.textContent = countdownTimer;

                if (countdownTimer <= 0) {{
                    updateMessageElement.textContent = 'Data updated successfully!';
                    updateMessageElement.style.color = 'green';

                    setTimeout(function() {{
                        updateMessageElement.textContent = '';
                    }}, 5000);

                    countdownTimer = 30;
                }}
            }}

            setInterval(updateCountdown, 1000);
        </script>
    </body>
    </html>";

    return Results.Content(result, "text/html");
});

app.Run();
