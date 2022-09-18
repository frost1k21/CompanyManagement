using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Core.Services;

public class DepartmentService : IDepartmentService
{
    private readonly DataContext _dataContext;

    public DepartmentService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<Department>> GetDepartments(string parentDepartmentTitle)
    {
        if (string.IsNullOrEmpty(parentDepartmentTitle))
        {
            return await _dataContext.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
                .ToListAsync();
        }

        var parentDepartment = await GetDepartmentByTitle(parentDepartmentTitle);
        if (parentDepartment is null)
            return Enumerable.Empty<Department>();
        
        return await _dataContext.Departments
            .Where(d => d.HeadDepartment == parentDepartment)
            .Include(d => d.Employees)
            .ThenInclude(e => e.Position)
            .ToListAsync();
    }

    public async Task<Department> GetDepartmentById(int id)
    {
        return await _dataContext.Departments
            .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Department> GetDepartmentByTitle(string title)
    {
        return await _dataContext.Departments
            .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
            .FirstOrDefaultAsync(d =>
                EF.Functions.ILike(d.Title, title));
    }

    public async Task<Department> CreateDepartment(string title)
    {
        var department = new Department() { Title = title };
        var result = _dataContext.Departments.Add(department);
        await _dataContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task UpdateDepartment(Department department)
    {
        _dataContext.Departments.Update(department);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteDepartment(Department department)
    {
        _dataContext.Departments.Remove(department);
        await _dataContext.SaveChangesAsync();
    }

    public DepartmentStats GetStats(Department department)
    {
        var stats = new DepartmentStats();
        stats.GetStatsFromDepartment(department);
        return stats;
    }

    public async Task SaveDepartments(List<Department> departments)
    {
        _dataContext.Departments.AddRange(departments);
        await _dataContext.SaveChangesAsync();
    }
}