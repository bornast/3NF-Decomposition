using _3NF.Decomposition.Core.Interfaces;
using _3NF.Decomposition.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3NF.Decomposition.Persistance
{
    public static class DependecyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(x => {
                x.UseLazyLoadingProxies();
                x.UseSqlServer(configuration.GetConnectionString("Database"));
            });

            services.AddScoped<IRepository, Repository>();
        }
    }
}
