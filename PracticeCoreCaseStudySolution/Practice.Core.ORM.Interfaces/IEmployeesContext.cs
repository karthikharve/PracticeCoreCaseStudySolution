using Microsoft.EntityFrameworkCore;
using Practice.Core.Models;
using System;

namespace Practice.Core.ORM.Interfaces
{
    public interface IEmployeesContext : ISystemContext
    {
        DbSet<Employees> Employees { get; set; }
    }
}
