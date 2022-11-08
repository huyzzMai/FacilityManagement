using BusinessObject.Commons;
using BusinessObject.RequestModel.DepartmentRequest;
using BusinessObject.ResponseModel.DepartmentResponse;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.DepartmentController
{
    [Route("api/department")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        public DepartmentController (IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartment()
        {
            try
            {
                return Ok(await departmentService.GetAllDepartments());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmentById(int id){
            try
            {
                var d = await departmentService.GetDepartmentById(id);
                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }

                string status = null;
                if (d.Status == CommonEnums.DEPARTMENTSTATUS.ACTIVE)
                {
                    status = "ACTIVE";
                }
                else if (d.Status == CommonEnums.DEPARTMENTSTATUS.BUSY)
                {
                    status = "BUSY";
                }

                var department = new DepartmentResponse()
                {
                    Id = d.Id,
                    DepartmentName = d.Name,
                    Status = status
                };

                return Ok(department);

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentRequest model)
        {
            try
            {
                var d = await departmentService.GetDepartmentByName(model.DepartmentName);

                if (d != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Department already exist!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status201Created,
                    await departmentService.CreateDepartment(model));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentRequest model)
        {
            try
            {
                var check = await departmentService.GetDepartmentByName(model.DepartmentName);
                if(check != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Department name already existed!");
                }

                var d = await departmentService.GetDepartmentById(id);

                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await departmentService.UpdateDepartment(id, model);
                    return Ok("Department updated successfully!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("status/busy/{id:int}")]
        public async Task<IActionResult> UpdateBusyDepartmentStatus(int id)
        {
            try
            {         
                await departmentService.AddBusyStatus(id);
                 return Ok("Status updated successfully.");   
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("status/remove-busy/{id:int}")]
        public async Task<IActionResult> RemoveBusyDepartmentStatus(int id)
        {
            try
            {
                await departmentService.RemoveBusyStatus(id);
                return Ok("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var d = await departmentService.GetDepartmentById(id);

                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await departmentService.DeleteDepartment(id);
                    return Ok("Delete successfully!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
