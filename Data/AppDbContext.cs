using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<PlayerScore> PlayerScores { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
