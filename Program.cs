using System.IO;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Data;
using MyAspNetCoreApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

 
var isAzure = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

 
var dbPath = isAzure
    ? Path.Combine("D:\\home\\site\\wwwroot", "tictactoe.db")  
    : Path.Combine(builder.Environment.ContentRootPath, "tictactoe.db");  


 
if (!File.Exists(dbPath))
{
    using (File.Create(dbPath)) { }  
}

 
if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    File.SetUnixFileMode(dbPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
}

 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


builder.Services.AddControllers();
builder.Services.AddSignalR();  


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",                       
            "https://snazzy-gecko-e98550.netlify.app",  
            "https://tictactoe2.azurewebsites.net"       
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

 
app.UseRouting();
app.UseCors();  
app.UseAuthorization();
app.MapHub<GameHub>("/gamehub");  
app.MapControllers();
app.Run();
