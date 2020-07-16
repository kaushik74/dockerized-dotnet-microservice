using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeProvider employeeProvider;
        public EmployeeController(IEmployeeProvider employeeProvider)
        {
            this.employeeProvider = employeeProvider;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            var res = await employeeProvider.GetEmployeesAsync();
            if (res.IsSuccess)
            {
                return Ok(res.employees);
            }
            return NotFound(res.ErrorMessage);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("GetEmployee/{id}")]
        public async Task<ActionResult> GetEmployee(int id)
        {
            var res = await employeeProvider.GetEmployeeAsync(id);
            if (res.IsSuccess)
            {
                return Ok(res.employee);
            }
            return NotFound(res.ErrorMessage);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] Database.Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await employeeProvider.AddEmployeeAsync(employee);
                    if (res.IsSuccess)
                    {
                        return Ok(res.EmployeeId);
                    }
                    return NotFound(res.ErrorMessage);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("UpdateEmployee/{Id}")]
        public async Task<ActionResult> UpdateEmployee(int Id, [FromBody] Database.Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await employeeProvider.UpdateEmployeeAsync(Id, employee);
                    if (res.IsSuccess)
                    {
                        return Ok(res.EmployeeId);
                    }
                    return NotFound(res.ErrorMessage);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("DeleteEmployee/{Id}")]
        public async Task<ActionResult> DeleteEmployee(int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await employeeProvider.DeleteEmployeeAsync(Id);
                    if (res.IsSuccess)
                    {
                        return Ok(null);
                    }
                    return NotFound(res.ErrorMessage);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
