using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Cryptography.X509Certificates;

namespace TaskFlow.IntegrationTests.Infrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public IServiceProvider sevices => Server.Services;

        public DatabaseFixture Database { get; } = new();

        private readonly PostgreSqlContainer _dbContainer = 
            new PostgreSqlBuilder("postgres:17")
            .WithDatabase("taskflow_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<TaskFlowDbContext>>();

                services.AddDbContext<TaskFlowDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                var provider = services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();

                db.Database.Migrate();

                Database.InitializeAsync(db)
                    .GetAwaiter()
                    .GetResult();
            });
        }
    }
}
