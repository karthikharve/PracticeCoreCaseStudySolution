using Practice.Core.DataAccess.Interfaces;
using Practice.Core.Models;
using Practice.Core.ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Practice.Core.DataAccess.Implementations
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private IEmployeesContext employeesContext  = default(IEmployeesContext);

        public EmployeesRepository(IEmployeesContext empContext)
        {
            this.employeesContext = empContext;

            if(empContext == default(IEmployeesContext))
            {
                throw new ArgumentException(paramName: nameof(empContext), message: "SPECIFIED EMPLOYEE CONTEXT IS INVALID");
            }
        }

        public int AddEmployee(Employees newEmployeeRecord)
        {
            int result = 0;

            if(newEmployeeRecord != null && !string.IsNullOrEmpty(newEmployeeRecord.FirstName))
            {
                var addedEntity = this.employeesContext.Employees.Add(newEmployeeRecord);
                var rowAffected = this.employeesContext.CommitChanges();

                if(rowAffected > 0)
                {
                    result = addedEntity.Entity.ID;
                }
            }

            return result;
        }

        public void Dispose() => this.employeesContext?.Dispose();

        /*
        public void Dispose()
        {
            if(this.employeesContext != default(IEmployeesContext))
            {
                this.employeesContext.Dispose();
            }
        }
        */

        public Employees GetEmployeeById(int Id)
        {
            return this.employeesContext.Employees.Where(emp => emp.ID == Id).FirstOrDefault();
        }

        public List<Employees> GetEmployees()
        {
            return this.employeesContext.Employees.ToList<Employees>();
        }

        public List<Employees> GetEmployeesByName(string employeeName)
        {
            var filterList = default(List<Employees>);

            filterList = this.employeesContext.ExecuteProcedure<Employees>("dbo.GetEmployeesByName", new Dictionary<string, object>()
            {
                ["@EmpName"] = employeeName
            });
            
            return filterList;
        }
    }
}
