// Employee Data Transfer Object (DTO)
//add namespace
namespace EmployeeManagementAPI.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }


    public class EmployeeCreateDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
}