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
            List<User> users = await dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserAndDeleteIsFalse(int id)
        {
            User us = await dbContext.Users.Where(u => u.Id.Equals(id) && u.IsDeleted == false)
                .FirstOrDefaultAsync();
            return us;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            User user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;    
        }

        // Use when delete or update user
        public async Task SaveUser(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
        
        // Save user for creation
        public async Task SaveCreateUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
