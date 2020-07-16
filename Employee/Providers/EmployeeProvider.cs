using AutoMapper;
using Employee.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.API.Database;
using Employee.API.Models;

namespace Employee.API.Providers
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private readonly Database.EmployeeDBContext dBContext;
        private readonly ILogger<EmployeeProvider> logger;
        private readonly IMapper mapper;

        public EmployeeProvider(Database.EmployeeDBContext dBContext, ILogger<EmployeeProvider> logger, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<(bool IsSuccess, int EmployeeId, string ErrorMessage)> AddEmployeeAsync(Database.Employee employee)
        {
            try
            {
                logger?.LogInformation("Adding Employee Data");
                if(employee != null)
                {
                    //Adding Employee and Employee Address data
                    await dBContext.Employees.AddRangeAsync(employee);

                    foreach(EmployeeAddress employeeAddress in employee.EmployeeAddresses)
                    {
                        await dBContext.EmployeeAddresses.AddAsync(employeeAddress);
                    }

                    await dBContext.SaveChangesAsync();
                    return (true, employee.Id, null);
                }
                return (false, 0, "No Records Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, 0, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteEmployeeAsync(int Id)
        {
            try
            {
                logger?.LogInformation($"Deleting employee details with id:{Id}");
                var employees = await dBContext.Employees
                    .Include(c => c.EmployeeAddresses)
                    .ToListAsync();

                var employee = await dBContext.Employees.FirstOrDefaultAsync(c => c.Id == Id);
                if (employee != null)
                {
                    logger?.LogInformation("Employee Found");
                    var result = dBContext.Remove(employee);
                    await dBContext.SaveChangesAsync();
                    return (true, null);
                }
                return (false, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Database.Employee employee, string ErrorMessage)> GetEmployeeAsync(int Id)
        {
            try
            {
                logger?.LogInformation($"Getting employee details with id:{Id}");
                var employees = await dBContext.Employees
                    .Include(c => c.EmployeeAddresses)
                    .ToListAsync();

                var employee = await dBContext.Employees.FirstOrDefaultAsync(c => c.Id == Id);
                if(employee != null)
                {
                    logger?.LogInformation("Employee Found");
                    var result = mapper.Map<Database.Employee>(employee);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Database.Employee> employees, string ErrorMessage)> GetEmployeesAsync()
        {
            try
            {
                logger?.LogInformation("Quering Employee Database");
                var employees = await dBContext.Employees
                    .Include(c => c.EmployeeAddresses)
                    .ToListAsync();

                if (employees != null && employees.Any())
                {
                    logger?.LogInformation($"{ employees.Count} employees found");
                    var result = mapper.Map<IEnumerable<Database.Employee>>(employees);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, int EmployeeId, string ErrorMessage)> UpdateEmployeeAsync(int Id, Database.Employee employee)
        {
            try
            {
                logger?.LogInformation($"Updating Employee {Id} record");
                if(employee != null)
                {
                    //Updating EmployeeAddress 
                    foreach (EmployeeAddress employeeAddress in employee.EmployeeAddresses) 
                    {
                        dBContext.EmployeeAddresses.UpdateRange(employeeAddress);
                    }

                    dBContext.Entry(employee).State = EntityState.Modified;
                    dBContext.Update(employee);
                    await dBContext.SaveChangesAsync();

                    return (true, employee.Id, null);
                }
                return (false, 0, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, 0, ex.Message);
            }
        }
    }
}
