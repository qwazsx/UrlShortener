using Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=UrlShortener.db");
        }

        public DbSet<UrlInfo> UrlInfos { get; set; }
    }
}
