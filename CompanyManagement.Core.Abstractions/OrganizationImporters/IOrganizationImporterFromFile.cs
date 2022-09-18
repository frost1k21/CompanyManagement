using CompanyManagement.Domain.Models;

namespace CompanyManagement.Core.Abstractions.OrganizationImporters
{
    public interface IOrganizationImporterFromFile
    {
        Task CreateOrUpdateDepartmentsFromFile(string filePath);
    }
}
