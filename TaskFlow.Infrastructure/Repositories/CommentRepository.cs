using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Enities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TaskFlowDbContext _context;

        public CommentRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
           _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(x => x.TaskItemId == taskId)
                .ToListAsync();
        }
    }
}
