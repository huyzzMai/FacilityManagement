using BusinessObject.Models;
using BusinessObject.RequestModel.DepartmentRequest;
using BusinessObject.ResponseModel.DepartmentResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllDepartments();
        Task<Department> GetDepartmentById(int id);
        Task<Department> GetDepartmentByName(string name);
        Task<DepartmentResponse> CreateDepartment(DepartmentRequest request);
        Task<DepartmentResponse> UpdateDepartment(int id, DepartmentRequest request);
        Task UpdateDepartmentStatus(int id, int request);
        Task DeleteDepartment(int id);

    }
}
