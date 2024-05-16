using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Context;
using TestsBuilder.Services;

namespace TestsBuilder
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection
services)
        {
            services.AddSingleton<IDbService, AuthService>();
            return services;
        }

        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            DbContextOptions options)
        {
            services.AddPersistence()
            .AddSingleton<AppDbContext>(
           new AppDbContext((DbContextOptions<AppDbContext>)options));
            return services;
        }
    }

}
