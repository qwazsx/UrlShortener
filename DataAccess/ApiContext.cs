using Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "UrlShortenerDatabase");
            optionsBuilder.UseInMemoryDatabase("UserContextWithNullCheckingDisabled", b => b.EnableNullChecks(false));
        }

        public DbSet<UrlInfo> UrlInfos { get; set; }
    }
}