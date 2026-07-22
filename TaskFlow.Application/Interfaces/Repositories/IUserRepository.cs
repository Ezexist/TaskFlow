using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUserNameAsync(string userName);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);
    }
}
