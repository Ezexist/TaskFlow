using Microsoft.EntityFrameworkCore;
using Respawn;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.IntegrationTests.Infrastructure
{
    public class DatabaseFixture
    {
        private Respawner? _respawner;

        public async Task InitializeAsync(TaskFlowDbContext db)
        {
            await db.Database.OpenConnectionAsync();

            _respawner = await Respawner.CreateAsync(
                db.Database.GetDbConnection(),
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres
                });

            await db.Database.CloseConnectionAsync();
        }

        public async Task ResetAsync(TaskFlowDbContext db)
        {
            if (_respawner == null)
            {
                throw new InvalidOperationException("Respawner is not initialized");

            }
            await db.Database.OpenConnectionAsync();

            await _respawner.ResetAsync(db.Database.GetDbConnection());

            await db.Database.CloseConnectionAsync();
        }
    }
}
