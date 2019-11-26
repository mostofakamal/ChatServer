using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Shared;

namespace WebApi.Infrastructure.Data
{
    public class GameDbContextFactory : DesignTimeDbContextFactoryBase<GameDbContext>
    {
        protected override GameDbContext CreateNewInstance(DbContextOptions<GameDbContext> options)
        {
            return new GameDbContext(options);
        }
    }
}