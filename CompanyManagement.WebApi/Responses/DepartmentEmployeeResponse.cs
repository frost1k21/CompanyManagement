using CompanyManagement.Domain.Models;

namespace CompanyManagement.WebApi.Responses;

public class DepartmentEmployeeResponse
{
    public DepartmentEmployeeResponse(Employee employee)
    {
        Id = employee.Id;
        FullName = employee.FullName;
        PositionTitle = employee.Position.Title;
    }
    
    public int Id { get; }
    public string FullName { get; }
    public string PositionTitle { get; }
}