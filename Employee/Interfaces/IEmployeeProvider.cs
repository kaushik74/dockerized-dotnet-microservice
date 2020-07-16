using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.API.Database;

namespace Employee.API.Interfaces
{
    public interface IEmployeeProvider
    {
        Task<(bool IsSuccess, IEnumerable<Employee.API.Database.Employee> employees, string ErrorMessage)> GetEmployeesAsync();

        Task<(bool IsSuccess, Employee.API.Database.Employee employee, string ErrorMessage)> GetEmployeeAsync(int Id);

        Task<(bool IsSuccess, int EmployeeId, string ErrorMessage)> AddEmployeeAsync(Database.Employee employee);

        Task<(bool IsSuccess, int EmployeeId, string ErrorMessage)> UpdateEmployeeAsync(int Id, Database.Employee employee);

        Task<(bool IsSuccess, string ErrorMessage)> DeleteEmployeeAsync(int Id);

    }
}
