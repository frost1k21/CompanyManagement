using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.WebApi.Requests;

public class UpdateEmployeeRequest
{
    public string FullName { get; set; }
    public string DepartmentTitle { get; set; }
    public string PositionTitle { get; set; }
}