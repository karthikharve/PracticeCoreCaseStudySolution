using Practice.Core.Models;
using System;
using System.Collections.Generic;

namespace Practice.Core.DataAccess.Interfaces
{
    public interface IEmployeesRepository : IDisposable
    {
        List<Employees> GetEmployees();
        List<Employees> GetEmployeesByName(string employeeName);
        Employees GetEmployeeById(int Id);
        int AddEmployee(Employees newEmployeeRecord);
    }
}
