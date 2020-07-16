using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Profiles
{
    public class EmployeeProfile :AutoMapper.Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Database.Employee, Models.EmployeeModel>();
            CreateMap<Database.EmployeeAddress, Models.EmployeeAddressModel>();
        }
    }
}
