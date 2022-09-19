using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.WebApi.Requests;
using CompanyManagement.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.WebApi.Controllers;

[ApiController]
[Route("employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly IPositionService _positionService;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        IPositionService positionService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _positionService = positionService;
    }

    [HttpGet("getemployees")]
    public async Task<IEnumerable<EmployeeResponse>> GetEmployees([FromQuery] string fullName, [FromQuery] int pageNumber = 1)
    {
        var employees = await _employeeService.GetEmployees(fullName, pageNumber);
        return employees.Select(e => new EmployeeResponse(e));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee is null)
            return NotFound();
        return Ok(new EmployeeResponse(employee));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest createEmployeeRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var department = await _departmentService.GetDepartmentByTitle(createEmployeeRequest.DepartmentTitle);
        if (department is null)
            return NotFound($@"No such department ""{createEmployeeRequest.DepartmentTitle}""");

        var position = await _positionService.GetPositionByTitle(createEmployeeRequest.PositionTitle);
        if (position is null)
            return NotFound($@"No such position ""{createEmployeeRequest.PositionTitle}""");

        var employee = new Employee() { Department = department, Position = position, FullName = createEmployeeRequest.FullName};
        await _employeeService.CreateEmployee(employee);
        return Ok(new EmployeeResponse(employee));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee is null)
            return NotFound();
        await _employeeService.DeleteEmployee(employee);
        return Ok();
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest updateEmployeeRequest)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee is null)
            return NotFound();

        var departmentTitleIsEmpty = string.IsNullOrEmpty(updateEmployeeRequest.DepartmentTitle);
        var positionTitleIsEmpty = string.IsNullOrEmpty(updateEmployeeRequest.PositionTitle);
        var fullNameIsEmpty = string.IsNullOrEmpty(updateEmployeeRequest.FullName);

        if (departmentTitleIsEmpty
            && positionTitleIsEmpty
            && fullNameIsEmpty)
            return BadRequest("At least one field must be is set");
        
        if (!departmentTitleIsEmpty)
        {
            var department = await _departmentService.GetDepartmentByTitle(updateEmployeeRequest.DepartmentTitle);
            if (department is null)
                return NotFound($@"No such department ""{updateEmployeeRequest.DepartmentTitle}""");
            employee.Department = department;
        }

        if (!positionTitleIsEmpty)
        {
            var position = await _positionService.GetPositionByTitle(updateEmployeeRequest.PositionTitle);
            if (position is null)
                return NotFound($@"No such department ""{updateEmployeeRequest.PositionTitle}""");
            employee.Position = position;
        }

        if (!fullNameIsEmpty)
        {
            employee.FullName = updateEmployeeRequest.FullName;
        }

        await _employeeService.UpdateEmployee(employee);
        return Ok(new EmployeeResponse(employee));
    }
}