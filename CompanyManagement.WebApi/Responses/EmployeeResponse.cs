using CompanyManagement.Domain.Models;

namespace CompanyManagement.WebApi.Responses;

public class EmployeeResponse
{
    public EmployeeResponse(Employee employee)
    {
        Id = employee.Id;
        FullName = employee.FullName;
        PositionTitle = employee.Position.Title;
        DepartmentTitle = employee.Department.Title;
    }

    public int Id { get; }
    public string FullName { get; }
    public string PositionTitle { get; }
    public string DepartmentTitle { get; }
}