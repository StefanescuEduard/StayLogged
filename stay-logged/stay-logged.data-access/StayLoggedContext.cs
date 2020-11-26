using Microsoft.EntityFrameworkCore;
using StayLogged.Domain;

namespace stay_logged.data_access
{
    public sealed class StayLoggedContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public DbSet<Host> Hosts { get; set; }

        public StayLoggedContext(DbContextOptions<StayLoggedContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Host>()
                .Property(r => r.Ip);
        }
    }
}
