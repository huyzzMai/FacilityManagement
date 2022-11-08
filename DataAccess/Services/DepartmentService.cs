using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.DepartmentRequest;
using BusinessObject.ResponseModel.DepartmentResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;   
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartments()
        {
            var departments = await departmentRepository.GetAllDepartments();
            IEnumerable<DepartmentResponse> result = departments.Select(
                department =>
                {
                    string status;
                    if (department.Status == CommonEnums.DEPARTMENTSTATUS.ACTIVE)
                    {
                        status = "Active";
                    }
                    else if (department.Status == CommonEnums.DEPARTMENTSTATUS.BUSY)
                    {
                        status = "Busy";
                    }
                    else
                    {
                        status = "Inactive";
                    }

                    return new DepartmentResponse()
                    {
                       Id = department.Id,
                       DepartmentName = department.Name,
                       Status = status
                    };
                }
                )
                .ToList();
            return result;
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            Department department = await departmentRepository.GetDepartmentAndDeleteIsFalse(id);
            return department;
        }

        public async Task<Department> GetDepartmentByName(string name)
        {
            Department department = await departmentRepository.GetDepartmentByNameAndDeleteIsFalse(name);
            return department;
        }

        public async Task<DepartmentResponse> CreateDepartment(DepartmentRequest request)
        {
            Department d = new Department()
            {
                Name = request.DepartmentName,
                Status = CommonEnums.DEPARTMENTSTATUS.ACTIVE,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
                
            };

            await departmentRepository.SaveCreateDepartment(d);

            var upde = new DepartmentResponse()
            {
                Id = d.Id,
                DepartmentName = d.Name,
                Status = "Active"
            };

            return upde;
        }

        public async Task<DepartmentResponse> UpdateDepartment(int id, DepartmentRequest request)
        {
            Department d = await departmentRepository.GetDepartmentAndDeleteIsFalse(id);

            if (d == null)
            {
                throw new Exception("This department cannot be update!");
            }

            d.Name = request.DepartmentName;
            d.UpdatedAt = DateTime.Now;
            d.UpdatedBy = "Admin";

            await departmentRepository.SaveDepartment(d);

            var upde = new DepartmentResponse()
            {   
                Id = d.Id,
                DepartmentName = d.Name,
                Status = "Active"
            };

            return upde;
        }

        public async Task AddBusyStatus(int id)
        {
            Department d = await departmentRepository.GetDepartmentAndDeleteIsFalse(id);
            if (d == null)
            {
                throw new Exception("This department is unavailable to update.");
            }

            if (d.Status != CommonEnums.DEPARTMENTSTATUS.BUSY) 
            {
                  d.Status = CommonEnums.DEPARTMENTSTATUS.BUSY;
                  d.UpdatedAt = DateTime.Now;
                  d.UpdatedBy = "Admin";
                  await departmentRepository.SaveDepartment(d); 
            }
            else
            {
                throw new Exception("This department already busy!");
            }
        }

        public async Task RemoveBusyStatus(int id)
        {
            Department d = await departmentRepository.GetDepartmentAndDeleteIsFalse(id);
            if (d == null)
            {
                throw new Exception("This department is unavailable to update.");
            }

            if (d.Status == CommonEnums.DEPARTMENTSTATUS.BUSY)
            {
                d.Status = CommonEnums.DEPARTMENTSTATUS.ACTIVE;
                d.UpdatedAt = DateTime.Now;
                d.UpdatedBy = "Admin";
                await departmentRepository.SaveDepartment(d);
            }
            else
            {
                throw new Exception("This department already active!");
            }
        }

        public async Task DeleteDepartment(int id)
        {
            Department d = await departmentRepository.GetDepartmentAndDeleteIsFalse(id);

            if (d == null)
            {
                throw new Exception("This department is unavailable to delete.");
            }

            d.IsDeleted = true;
            d.Status = CommonEnums.DEPARTMENTSTATUS.INACTIVE;
            d.UpdatedAt = DateTime.Now;
            d.UpdatedBy = "Admin";

            await departmentRepository.SaveDepartment(d);
        }

    }
}

