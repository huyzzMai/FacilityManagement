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
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetFixerList();
        // Get active user
        Task<User> GetUserAndDeleteIsFalse(int id);
        Task<User> GetUserByEmailAndDeleteIsFalse(string email);
        // Use when delete or update user
        Task SaveUser(User user);
        // Save user for creation
        Task SaveCreateUser(User user); 
        User GetUserAndDeleteIsFalseNoTask(int id);
    }
}
