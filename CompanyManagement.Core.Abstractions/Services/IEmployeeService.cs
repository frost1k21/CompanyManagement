using CompanyManagement.Domain.Models;

namespace CompanyManagement.Core.Abstractions.Services;

public interface IEmployeeService
{
    public Task<List<Employee>> GetEmployees(string fullName);
    public Task<Employee> GetEmployeeById(int id);
    public Task<Employee> CreateEmployee(Employee employee);
    public Task UpdateEmployee(Employee employee);
    public Task DeleteEmployee(Employee employee);
}