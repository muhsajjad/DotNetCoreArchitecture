using DotNetCore.IoC;
using DotNetCoreArchitecture.Application;
using DotNetCoreArchitecture.Database;
using DotNetCoreArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DotNetCoreArchitecture.IoC
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogger(configuration);
            services.AddHash();
            services.AddCriptography(Guid.NewGuid().ToString());
            services.AddJsonWebToken(Guid.NewGuid().ToString());

            services.AddClassesMatchingInterfacesFrom
            (
                Assembly.GetAssembly(typeof(IAuthenticationApplication)),
                Assembly.GetAssembly(typeof(IAuthenticationDomain)),
                Assembly.GetAssembly(typeof(IDatabaseUnitOfWork))
            );

            services.AddDbContextEnsureCreatedMigrate<DatabaseContext>(options => options
                .UseSqlServer(configuration.GetConnectionString(nameof(DatabaseContext)))
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
            );
        }
    }
}
