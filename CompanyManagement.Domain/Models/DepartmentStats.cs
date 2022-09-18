using System.Text.Json.Serialization;

namespace CompanyManagement.Domain.Models;

public class DepartmentStats
{
    public int EmployeesAmount { get; private set; }
    public int PositionsAmount { get; private set; }

    public void GetStatsFromDepartment(Department department)
    {
        EmployeesAmount = department.Employees.Count;
        PositionsAmount = department.Employees.Select(e => e.Position).Distinct().Count();
    }
}