using CompanyManagement.Domain.Models;

namespace CompanyManagement.WebApi.Responses;

public class DepartmentResponse
{
    public DepartmentResponse(Department department)
    {
        Id = department.Id;
        Title = department.Title;
        HeadDepartment = department.HeadDepartment?.Title;
        Employees = MapFromEmployees(department.Employees);
    }

    public int Id { get; }
    public string Title { get; }
    public string HeadDepartment { get; }
    public List<DepartmentEmployeeResponse> Employees { get; }

    private List<DepartmentEmployeeResponse> MapFromEmployees(List<Employee> employees)
    {
        if (!employees.Any())
            return new List<DepartmentEmployeeResponse>();
        return employees.Select(e => new DepartmentEmployeeResponse(e)).ToList();
    }
}