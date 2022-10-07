using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<List<User>> GetAllUsers();
        // Get active user
        Task<User> GetUserAndDeleteIsFalse(int id);
        Task SaveUser(User user);
    }
}
