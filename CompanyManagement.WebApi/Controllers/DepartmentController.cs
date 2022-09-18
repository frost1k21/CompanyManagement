using CompanyManagement.Core.Abstractions.OrganizationImporters;
using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.WebApi.Requests;
using CompanyManagement.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.WebApi.Controllers;

[ApiController]
[Route("department")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IOrganizationImporterFromFile _organizationImporterFromFile;
    private readonly IWebHostEnvironment _hostEnvironment;

    public DepartmentController(
        IDepartmentService departmentService,
        IOrganizationImporterFromFile organizationImporterFromFile,
        IWebHostEnvironment hostEnvironment
        )
    {
        _departmentService = departmentService;
        _organizationImporterFromFile = organizationImporterFromFile;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet("getdepartments")]
    public async Task<IEnumerable<DepartmentResponse>> GetDepartments([FromQuery] string parentDepartment)
    {
        var result = await _departmentService.GetDepartments(parentDepartment);
        if (!result.Any())
            return Enumerable.Empty<DepartmentResponse>();
        return result.Select(d => new DepartmentResponse(d));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await _departmentService.GetDepartmentById(id);
        if (department is null)
            return NotFound();
        var response = new DepartmentResponse(department);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetDepartmentByTitle([FromQuery] string title)
    {
        var department = await _departmentService.GetDepartmentByTitle(title);
        if (department is null)
            return NotFound();
        var response = new DepartmentResponse(department);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateOrUpdateTitle titleRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var title = titleRequest.Title;
        var department = await _departmentService.GetDepartmentByTitle(title);
        if (department != null)
            return BadRequest($@"Department ""{title}"" already exists");
        department = await _departmentService.CreateDepartment(title);
        var response = new DepartmentResponse(department);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] CreateOrUpdateTitle titleRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var department = await _departmentService.GetDepartmentById(id);
        if (department is null)
            return NotFound();
        var title = titleRequest.Title;
        department.Title = title;
        await _departmentService.UpdateDepartment(department);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await _departmentService.GetDepartmentById(id);
        if (department is null)
            return NotFound();
        await _departmentService.DeleteDepartment(department);
        return Ok();
    }

    [HttpGet("stats/{id}")]
    public async Task<IActionResult> GetStats(int id)
    {
        var department = await _departmentService.GetDepartmentById(id);
        if (department is null)
            return NotFound();
        var stats = _departmentService.GetStats(department);
        return Ok(stats);
    }

    [HttpPost("importfromfile")]
    public async Task<IActionResult> ImportFromFile(IFormFile file)
    {
        var uploads = Path.Combine(_hostEnvironment.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploads);
        var filePath = Path.Combine(uploads, $"{Guid.NewGuid()}-{file.FileName}");
        var departments = new List<Department>();
        if (file.Length > 0)
        {
            await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
            await _organizationImporterFromFile.CreateOrUpdateDepartmentsFromFile(filePath);
        }
        return Accepted();
    }
}