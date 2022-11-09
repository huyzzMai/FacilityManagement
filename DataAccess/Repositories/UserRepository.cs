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

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await dbContext.Users
                .Where(u => u.IsDeleted == false).ToListAsync();
            return users;
        }

        public async Task<User> GetUserAndDeleteIsFalse(int id)
        {
            User us = await dbContext.Users.Where(u => u.Id.Equals(id) && u.IsDeleted == false) 
                .Include("Department")
                .FirstOrDefaultAsync();
            return us;
        }

        public async Task<User> GetUserByEmailAndDeleteIsFalse(string email)
        {
            User user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
            return user;
        }

        public async Task SaveUser(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
            
        public async Task SaveCreateUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        public User GetUserAndDeleteIsFalseNoTask(int id)
        {
            User us = dbContext.Users.Where(u => u.Id.Equals(id) && u.IsDeleted == false)
                .FirstOrDefault();
            return us;
        }
    }
}
