using BusinessObject.Models;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FacilityFeedbackManagementContext dbContext;

        public UserRepository(FacilityFeedbackManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            User user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;    
        }
    }
}
