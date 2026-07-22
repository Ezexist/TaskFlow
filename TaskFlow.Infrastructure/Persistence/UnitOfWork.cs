using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Others;

namespace TaskFlow.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly TaskFlowDbContext _context;

        public UnitOfWork(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
