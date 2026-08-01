using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Infrastructure.Persistence.Database
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync<TContext>(IServiceProvider services,ILogger logger,
            CancellationToken cancellationToken = default)
            where TContext : DbContext
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            const int maxRetries = 10;

            for(var attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    logger.LogInformation("Applying database migrations...");

                    await context.Database.MigrateAsync(cancellationToken);

                    logger.LogInformation("Database migration completed successfully");

                    return;
                }
                catch(Exception ex) when (
                ex is NpgsqlException || ex.InnerException is SocketException)
                {
                    logger.LogWarning(ex,
                        "Database is unavailable. Attempt {Attempt}/{MaxRetries}", attempt, maxRetries);

                    if (attempt == maxRetries)
                        throw;

                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
        }
    }
}
