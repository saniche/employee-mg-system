// Mapster mapping configuration for Employee and EmployeeDto
using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Models;
using Mapster;

namespace EmployeeManagementAPI.Mappings
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map entity -> DTO
            config.NewConfig<Employee, EmployeeDto>();

            // Map create DTO -> entity (ensure Id is not set by client)
            config.NewConfig<EmployeeCreateDto, Employee>()
                .Map(dest => dest.Id, src => 0);

            // Map update DTO -> entity (ignore nulls so partial updates don't overwrite with null)
            config.NewConfig<EmployeeUpdateDto, Employee>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.Id);
        }
    }
}