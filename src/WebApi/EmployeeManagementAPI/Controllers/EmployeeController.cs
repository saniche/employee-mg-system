// Employee Controller
using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        //define Get all employees endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDtos = employees.Adapt<IEnumerable<EmployeeDto>>();
            return Ok(employeeDtos);
        }

        //define Get employee by id endpoint
        [HttpGet("{id}")]
        //add response type attributes, possible 200 OK or 404 Not Found
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = employee.Adapt<EmployeeDto>();
            return Ok(employeeDto);
        }

        //define find employee pagination endpoint
        [HttpGet("paged")]
        //add response type attribute for 200 OK
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), 200)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetPagedEmployees([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var paged = await _employeeRepository.GetPagedAsync(pageNumber, pageSize);
            // Map the items collection only. PagedResult<T> cannot be adapted directly to IEnumerable<TDestination>.
            var employeeDtos = paged.Items.Adapt<IEnumerable<EmployeeDto>>();
            return Ok(employeeDtos);
        }

        //define create employee endpoint
        [HttpPost]
        //add response type attribute for 201 Created
        [ProducesResponseType(typeof(EmployeeDto), 201)]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeCreateDto employeeDto)
        {
            var employee = employeeDto.Adapt<Employee>();
            await _employeeRepository.AddAsync(employee);

            // adapt from saved entity so DTO includes generated Id and any DB-set fields
            var createdDto = employee.Adapt<EmployeeDto>();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, createdDto);
        }

        //define update employee endpoint
        [HttpPut("{id}")]
        //add response type attributes: 200 OK, 400 BadRequest, 404 NotFound
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest();

            // if client supplies an Id in the DTO, ensure it matches the route id
            if (employeeDto.Id != 0 && employeeDto.Id != id)
                return BadRequest("DTO id must match route id.");

            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // map incoming DTO into the existing entity
            employeeDto.Adapt(existingEmployee);

            await _employeeRepository.UpdateAsync(existingEmployee);

            var updatedEmployeeDto = existingEmployee.Adapt<EmployeeDto>();
            return Ok(updatedEmployeeDto);
        }

        //define delete employee endpoint
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            await _employeeRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
