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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly FacilityFeedbackManagementContext dbContext;

        public DepartmentRepository(FacilityFeedbackManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            List<Department> departments = await dbContext.Departments
                .Where(d => d.IsDeleted == false).ToListAsync();
            return departments;
        }

        public async Task<Department> GetDepartmentAndDeleteIsFalse(int id)
        {
            Department department = await dbContext.Departments.SingleOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);
            return department;
        }

        public async Task<Department> GetDepartmentByNameAndDeleteIsFalse(string name)
        {
            Department department = await dbContext.Departments.SingleOrDefaultAsync(d => d.Name == name && d.IsDeleted == false);
            return department;
        }

        public async Task SaveCreateDepartment(Department department)
        {
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveDepartment(Department department)
        {
            dbContext.Departments.Update(department);
            await dbContext.SaveChangesAsync();
        }
    }
}
