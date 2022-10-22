using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllDepartments();
        Task<Department> GetDepartmentAndDeleteIsFalse(int id);
        Task<Department> GetDepartmentByNameAndDeleteIsFalse(string name);
        // Use when delete or update department
        Task SaveDepartment(Department department);
        // Save department for creation
        Task SaveCreateDepartment(Department department);   
    }
}
