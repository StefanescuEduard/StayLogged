using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace stay_logged.data_access
{
    public class StayLoggedContextFactory : IDesignTimeDbContextFactory<StayLoggedContext>
    {
        public StayLoggedContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayLoggedContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=StayLogged;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new StayLoggedContext(optionsBuilder.Options);
        }
    }
}