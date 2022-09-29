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
    }
}
