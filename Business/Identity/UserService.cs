
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Identity;

namespace Business.Identity
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Add(User user)
        {
            return await _userRepository.Add(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
           return await _userRepository.GetAll();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<User?> GetById(Guid id)
        {
           return await _userRepository.GetById(id);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _userRepository.DeleteById(id);
        }

        public async Task<User> Update(User user)
        {
           return await _userRepository.Update(user);
        }
    }

    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(Guid id);

        Task<User?> GetByEmail(string email);


        Task<User> Add(User user);

        Task<User> Update(User user);

        Task<bool> Remove(Guid id);
    }
}
