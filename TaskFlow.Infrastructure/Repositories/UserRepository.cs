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
    public class UserRepository : IUserRepository
    {
        private readonly TaskFlowDbContext _context;
        public UserRepository(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            return _context.Users
                .AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            return await _context.Users
                .AnyAsync(x => x.UserName == userName);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id); 
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
