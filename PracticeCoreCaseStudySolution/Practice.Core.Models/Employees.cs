using System;

namespace Practice.Core.Models
{
    public class Employees
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }

        public override string ToString()
        {
            return $"ID: {this.ID}, Full Name : {this.FirstName ?? "" + "  " + this.MiddleName ?? "" + "  " + this.LastName ?? ""}, Salary : {this.Salary.ToString() ?? "Rs.0.0"}";
        }
    }
}
