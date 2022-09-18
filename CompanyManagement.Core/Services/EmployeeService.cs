using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly DataContext _dataContext;

    public EmployeeService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<List<Employee>> GetEmployees(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            return await _dataContext.Employees
                .Include(p=> p.Position)
                .Include(p => p.Department)
                .ToListAsync();
        return await _dataContext.Employees
            .Where(e => EF.Functions.ILike(e.FullName, fullName))
            .Include(e => e.Position)
            .Include(p => p.Department)
            .ToListAsync();
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        return await _dataContext.Employees
            .Include(e =>e.Position)
            .Include(p => p.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee> CreateEmployee(Employee employee)
    {
        _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateEmployee(Employee employee)
    {
        _dataContext.Employees.Update(employee);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteEmployee(Employee employee)
    {
        _dataContext.Employees.Remove(employee);
        await _dataContext.SaveChangesAsync();
    }
}