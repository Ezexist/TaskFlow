using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id);

        Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId);

        Task AddAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
