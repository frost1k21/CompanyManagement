using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.WebApi.Requests;

public class CreateEmployeeRequest
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string PositionTitle { get; set; }
    [Required]
    public string DepartmentTitle { get; set; }
}