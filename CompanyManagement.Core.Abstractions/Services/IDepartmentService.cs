using CompanyManagement.Domain.Models;

namespace CompanyManagement.Core.Abstractions.Services;

public interface IDepartmentService
{
    public Task<IEnumerable<Department>> GetDepartments(string parentDepartmentTitle, int pageNumber);
    public Task SaveDepartments(List<Department> departments);
    public Task<Department> GetDepartmentById(int id);
    public Task<Department> GetDepartmentByTitle(string title);
    public Task<Department> CreateDepartment(string title);
    public Task UpdateDepartment(Department department);
    public Task DeleteDepartment(Department department);
    public DepartmentStats GetStats(Department department);
}