using System.IO;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Data;
using MyAspNetCoreApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SQLite database setup
var dbPath = Path.Combine("/home", "TicTacToe.db"); 

// Ensure the database file exists
if (!File.Exists(dbPath))
{
    using (File.Create(dbPath)) { } // Properly close the file
}

// Set correct permissions on Linux/macOS
if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    File.SetUnixFileMode(dbPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
}
 

 
builder.Services.AddControllers();
builder.Services.AddSignalR();  
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",            // Allow localhost during development
            "https://snazzy-gecko-e98550.netlify.app"         // Allow Netlify when deployed
        )
              .AllowAnyHeader()
              .AllowAnyMethod()
               .AllowCredentials();  
    });
});

 
var app = builder.Build();

app.UseCors();
app.UseRouting();
app.MapHub<GameHub>("/gamehub");  
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

 
app.Run();
