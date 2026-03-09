using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions
{
    public static class DbServiceExtentions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Connection");

            return services.AddDbContext<AppDbContext>((options) => { options.UseSqlServer(conn); });
        }
    }
}
