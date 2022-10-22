﻿using BusinessObject.RequestModel.DepartmentRequest;
using DataAccess.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Controllers.DepartmentController
{
    [Route("api/department")]
    [ApiController]
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
                    return Ok(await departmentService.CreateDepartment(model));
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
                var d = await departmentService.GetDepartmentById(id);

                if (d == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    return Ok(await departmentService.UpdateDepartment(id, model));
                }
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
                    return Ok();
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