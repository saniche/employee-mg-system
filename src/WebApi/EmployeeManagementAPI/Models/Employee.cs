//generate class Employee with properties Id, Name, Position, and Salary
namespace EmployeeManagementAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        //add properties FirstName, Email,PhoneNumber
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
}