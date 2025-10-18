//generate class Employee with properties Id, Name, Position, and Salary
using System;

namespace EmployeeManagementAPI.Models
{
    public class Employee : EntityBase
    {
        public int Id { get; set; }
        public string LastName { get; set; } = default!;
        //add properties FirstName, Email,PhoneNumber
        public string FirstName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Position { get; set; } = default!;
        public decimal Salary { get; set; }

        public DateTime BirthDate { get; set; }

        // audit fields
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}