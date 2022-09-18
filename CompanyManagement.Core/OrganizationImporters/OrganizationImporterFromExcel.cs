using CompanyManagement.Core.Abstractions.OrganizationImporters;
using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.Persistence;
using OfficeOpenXml;

namespace CompanyManagement.Core.OrganizationImporters;

public class OrganizationImporterFromExcel : IOrganizationImporterFromFile
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly DataContext _dataContext;

    public OrganizationImporterFromExcel(
            IDepartmentService departmentService,
            IEmployeeService employeeService,
            IPositionService positionService,
            DataContext dataContext
        )
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
        _positionService = positionService;
        _dataContext = dataContext;
    }
    public async Task CreateOrUpdateDepartmentsFromFile(string filePath)
    {
        var departments = new Dictionary<string, Department>();
        var positions = new Dictionary<string, Position>();
        
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        await using var stream = File.Open(filePath, FileMode.Open);
        using var package = new ExcelPackage(stream);
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            // skip first row
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                var departmentTile = worksheet.Cells[i, 1].Value.ToString();
                var parentDepartmentTitle = worksheet.Cells[i, 2].Value?.ToString() ?? "";
                await AddDepartmentToDictionary(departments, departmentTile, parentDepartmentTitle);
                
                var positionTitle = worksheet.Cells[i, 3].Value.ToString();
                await AddPosition(positions, positionTitle);
                
                var fullName = worksheet.Cells[i, 4].Value.ToString();
                var employee = new Employee()
                {
                    FullName = fullName,
                    Position = positions[positionTitle],
                    Department = departments[departmentTile]
                };
                await _employeeService.CreateEmployee(employee);
            }
        }
    }

    private async Task AddPosition(Dictionary<string,Position> positions,
        string positionTitle)
    {
        if (positions.ContainsKey(positionTitle))
            return;
        var position = await _positionService.GetPositionByTitle(positionTitle);
        if (position == null)
            position = await _positionService.CreatePosition(positionTitle);
        positions.Add(positionTitle, position);
    }

    private async Task AddDepartmentToDictionary(Dictionary<string,Department> departments, 
        string departmentTile, 
        string parentDepartmentTitle)
    {
        if (departments.ContainsKey(departmentTile))
            return;
        var department = await _departmentService.GetDepartmentByTitle(departmentTile);
        if (department == null)
            department = await _departmentService.CreateDepartment(departmentTile);
        if (!string.IsNullOrEmpty(parentDepartmentTitle))
        {
            if (!departments.ContainsKey(parentDepartmentTitle))
            {
                var newDepartment = await _departmentService.GetDepartmentByTitle(parentDepartmentTitle);
                if (newDepartment == null)
                    newDepartment = await _departmentService.CreateDepartment(parentDepartmentTitle);
                departments.Add(parentDepartmentTitle, newDepartment);
            }

            department.HeadDepartment = departments[parentDepartmentTitle];
            await _departmentService.UpdateDepartment(department);
        }
        departments.Add(departmentTile, department);
    }
}