using Microsoft.EntityFrameworkCore;
using Practice.Core.Models;
using Practice.Core.ORM.Interfaces;
using System;
using System.Collections.Generic;

namespace Practice.Core.ORM.Implementations
{
    public class EmployeesContext : DbContext, IEmployeesContext
    {
        public EmployeesContext(DbContextOptions<EmployeesContext> dBContextOptions) : base(dBContextOptions) { }

        public EmployeesContext() : base() { }

        public DbSet<Employees> Employees { get; set; }

        public int CommitChanges()
        {
            return this.SaveChanges();
        }

        public List<T> ExecuteProcedure<T>(string procedureName, IDictionary<string, object> procedureParameters)
        {
            var filteredList = default(List<Employees>);

            if (typeof(T) == typeof(Employees))
            {
                try
                {
                    this.Database.OpenConnection();

                    if(!string.IsNullOrEmpty(procedureName))
                    {
                        var cmd = this.Database.GetDbConnection().CreateCommand();
                        cmd.CommandText = procedureName;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if(procedureParameters != default(IDictionary<string, object>))
                        {
                            foreach(var param in procedureParameters)
                            {
                                var newParameter = cmd.CreateParameter();

                                newParameter.ParameterName = param.Key;
                                newParameter.Value = param.Value;

                                cmd.Parameters.Add(newParameter);
                            }

                            using (var reader = cmd.ExecuteReader())
                            {
                                filteredList = new List<Employees>();

                                while (reader.Read())
                                {
                                    filteredList.Add(new Employees()
                                    {
                                        ID = Convert.ToInt32(reader["ID"].ToString()),
                                        FirstName = reader["FirstName"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        MiddleName = reader["MiddleName"].ToString(),
                                        Salary = Convert.ToDecimal(reader["Salary"].ToString())
                                    });
                                }
                            }
                        }
                    }
                }
                finally
                {
                    this.Database.CloseConnection();
                }
            }
            return filteredList as List<T>;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employees>(entityEmployee =>
            {
                entityEmployee.HasKey(model => model.ID);
                entityEmployee.Property(model => model.ID).UseIdentityColumn();
                entityEmployee.ToTable("Employees");
            });
        }
    }
}
