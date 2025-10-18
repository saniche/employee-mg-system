// Employee Data Transfer Object (DTO)
//add namespace
namespace EmployeeManagementAPI.Dtos
{
    public class EmployeeBaseDto
    {
        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Position { get; set; } = default!;
        public decimal Salary { get; set; }
    }

    public class EmployeeDto : EmployeeBaseDto
    {
        public int Id { get; set; }
    }

    public class EmployeeUpdateDto : EmployeeDto
    {
        public DateTime BirthDate { get; set; }
    }


    public class EmployeeCreateDto : EmployeeBaseDto
    {
        public DateTime BirthDate { get; set; }
    }



    public class EmployeeDetailsDto : EmployeeBaseDto
    {
        public string FullName { get => $"{FirstName} {LastName}"; }
        public DateTime BirthDate { get; set; }
    }
}