using animes.Models;
using Microsoft.EntityFrameworkCore;

namespace animes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AnimeSearchLog> animeSearchLogs { get; set; }
    }
}
