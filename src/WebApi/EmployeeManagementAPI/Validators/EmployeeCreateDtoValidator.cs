using FluentValidation;
using EmployeeManagementAPI.Dtos;

namespace EmployeeManagementAPI.Validators
{
    public class EmployeeCreateDtoValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateDtoValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Position).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
        }
    }


    public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Position).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
        }
    }
}