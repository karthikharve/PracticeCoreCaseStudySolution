using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice.Core.DataAccess.Interfaces;
using Practice.Core.Models;

namespace Practice.Core.Applications.WebApplication.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IEmployeesRepository employeesRepository = default(IEmployeesRepository);

        public EmployeesController(IEmployeesRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;

            if(this.employeesRepository == default(IEmployeesRepository))
            {
                throw new ArgumentException(paramName: nameof(employeesRepository), message: "INVALID EMPLOYEE REPOSITORY SPECIFIED !!");
            }
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employeeList = this.employeesRepository.GetEmployees();

            return  Ok(employeeList);
        }

        [HttpGet("search")]
        public IActionResult GetEmployeeByName(string employeeName)        
        {
            if (string.IsNullOrEmpty(employeeName))
            {
                //throw new ArgumentException(paramName: nameof(employeeName), message: "Employee Name should not be null or empty");

                return BadRequest("Employee Name should not be null or empty");
            }

            var employeeList = this.employeesRepository.GetEmployeesByName(employeeName);

            if(employeeList != null && employeeList.Any())
            {
                return Ok(employeeList);
            }
            else
            {
                return NotFound("No Match Found for the specified text");
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            if(id == default(int))
            {
                return BadRequest("ID cannot be zero or null");
            }

            var employee = this.employeesRepository.GetEmployeeById(id);

            if(employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound("Employee Record Not Found");
            }
        }

        [HttpPost("save")]
        public IActionResult AddEmployee([FromBody] Employees employeesRecord, [FromForm] EmployeesDepartment employeesDepartment)
        {
            if(employeesDepartment == null)
            {
                return BadRequest("Employee Record is INVALID");
            }

            if(employeesRecord != null && !string.IsNullOrEmpty(employeesRecord.FirstName))
            {
                var newId = this.employeesRepository.AddEmployee(employeesRecord);

                if(newId > 0)
                {
                    return Created(new Uri($"//api//employees//get//{newId}"), newId);
                }
                else
                {
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return BadRequest("Employee Record is INVALID");
            }
        }
    }
}