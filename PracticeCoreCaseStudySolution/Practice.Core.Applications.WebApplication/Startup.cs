using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Practice.Core.Applications.WebApplication.Utility;
using Practice.Core.DataAccess.Implementations;
using Practice.Core.DataAccess.Interfaces;
using Practice.Core.ORM.Implementations;
using Practice.Core.ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice.Core.Applications.WebApplication
{
    public class Startup
    {
        private IConfigurationRoot configurationRoot = default(IConfigurationRoot);

        public Startup(IWebHostEnvironment hostingEvironment)
        {
            this.configurationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            if(serviceCollection != default(IServiceCollection))
            {
                serviceCollection.AddDbContext<EmployeesContext>(dboptionsBuilder =>
                {
                    dboptionsBuilder.UseSqlServer(connectionString: this.configurationRoot.GetSection("ConnectionStrings").GetValue<string>("defaultConnectionString"));
                });

                serviceCollection.AddScoped<IEmployeesContext, EmployeesContext>();
                serviceCollection.AddScoped<IEmployeesRepository, EmployeesRepository>();
                serviceCollection.AddMvc(options=>
                {
                    options.EnableEndpointRouting = false;
                    options.ModelBinderProviders.Insert(0, new EmployeesDepartmentModelBinderProvider());
                });                
            }
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment hostingEnvironment)
        {
            if(applicationBuilder != default(IApplicationBuilder) && hostingEnvironment != default(IWebHostEnvironment))
            {
                if (hostingEnvironment.IsDevelopment())
                {
                    applicationBuilder.UseDeveloperExceptionPage();                    
                }
                else
                    applicationBuilder.UseExceptionHandler(errorHandlingPath: "/Error");

                applicationBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "Default",
                        template: "{controller=Home}/{action=Index}/{id?}"
                        );
                });

                applicationBuilder.UseStaticFiles(
                    new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider($"{hostingEnvironment.ContentRootPath}/wwwroot/applib"),
                        RequestPath = new Microsoft.AspNetCore.Http.PathString("/lib")
                    });
            }
        }
    }
}
