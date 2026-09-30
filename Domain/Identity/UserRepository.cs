using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDBContext _dbContext;
        public UserRepository(IdentityDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<User> Add(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                throw new Exception("user not found");
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            var user = _dbContext.Users.FindAsync(id);
            return await user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = _dbContext.Users.Where(u => u.EmailAddress == email).FirstOrDefaultAsync();
            return await user;
        }

        public async Task<User> Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(Guid id);

        Task<User?> GetByEmail(string email);
        Task<User> Add(User user);

        Task<User> Update(User user);

        Task<bool> DeleteById(Guid id);
    }
}
