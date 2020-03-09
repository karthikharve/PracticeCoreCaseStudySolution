using Microsoft.AspNetCore.Mvc.ModelBinding;
using Practice.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice.Core.Applications.WebApplication.Utility
{
    public class EmployeesDepartmentModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var task = Task.Run(() =>
            {
                var registeredDepartments = new Dictionary<string, EmployeesDepartment>
                {
                    ["Dev"] = new EmployeesDepartment() { DeptId = 10, DepartmentName = "Development" },
                    ["QA"] = new EmployeesDepartment() { DeptId = 11, DepartmentName = "Quality" },
                    ["Manager"] = new EmployeesDepartment() { DeptId = 12, DepartmentName = "Management" }
                };

                if(bindingContext != default(ModelBindingContext) && bindingContext.HttpContext != null)
                {
                    var headers = bindingContext.HttpContext.Request.Headers;
                    var deptKey = headers["DeptKey"];

                    if (!string.IsNullOrEmpty(deptKey) && bindingContext.ModelType == typeof(EmployeesDepartment))
                    {
                        var dept = registeredDepartments[deptKey];

                        if(dept != null)
                        {
                            bindingContext.Result = ModelBindingResult.Success(dept);
                            bindingContext.Model = dept;
                        }
                        else
                        {
                            bindingContext.Result = ModelBindingResult.Failed();
                        }
                    }
                }
                
            });

            return task;
        }
    }
}
