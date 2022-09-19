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
    
    public async Task<List<Employee>> GetEmployees(string fullName, int pageNumber)
    {
        var itemsPerPage = 10;
        if (pageNumber < 0)
            pageNumber = 1;
        var skipItems = (pageNumber - 1) * itemsPerPage;
        if (string.IsNullOrEmpty(fullName))
            return await _dataContext.Employees
                .OrderBy(p => p.Id)
                .Skip(skipItems)
                .Take(itemsPerPage)
                .Include(p=> p.Position)
                .Include(p => p.Department)
                .ToListAsync();
        return await _dataContext.Employees
            .Where(e => EF.Functions.ILike(e.FullName, fullName))
            .OrderBy(p => p.Id)
            .Skip(skipItems)
            .Take(itemsPerPage)
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