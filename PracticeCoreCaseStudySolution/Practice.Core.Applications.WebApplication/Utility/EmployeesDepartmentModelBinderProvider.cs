using Microsoft.AspNetCore.Mvc.ModelBinding;
using Practice.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice.Core.Applications.WebApplication.Utility
{
    public class EmployeesDepartmentModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var modelBinder = default(IModelBinder);

            if (context != default(ModelBinderProviderContext))
            {
                if(context.Metadata.ModelType == typeof(EmployeesDepartment))
                {
                    modelBinder = new EmployeesDepartmentModelBinder();
                }
            }

            return modelBinder;
        }
    }
}
