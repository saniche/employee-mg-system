using System;

namespace EmployeeManagementAPI.Models
{
    // EntityBase interface for common audit properties
    public interface EntityBase
    {
        int Id { get; set; }

        DateTime? Created { get; set; }

        DateTime? Updated { get; set; }

        string? CreatedBy { get; set; }

        string? UpdatedBy { get; set; }
    }
}
