using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.IntegrationTests.Infrastructure
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected readonly CustomWebApplicationFactory Factory;
        protected readonly HttpClient Client;

        protected IntegrationTestBase(CustomWebApplicationFactory factory)
        {
            Factory = factory;
            Client = factory.CreateClient();
        }


        public async Task InitializeAsync()
        {
            using var scope = Factory.Services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<TaskFlowDbContext>();

            await Factory.Database.ResetAsync(db);
        }

        public Task DisposeAsync() => Task.CompletedTask;

    }
}
