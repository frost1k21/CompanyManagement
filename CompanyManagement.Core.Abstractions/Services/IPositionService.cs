using CompanyManagement.Domain.Models;

namespace CompanyManagement.Core.Abstractions.Services;

public interface IPositionService
{
    public Task<List<Position>> GetPositions(int pageNumber);
    public Task<Position> GetPositionById(int id);
    public Task<Position> GetPositionByTitle(string title);
    public Task<Position> CreatePosition(string title);
    public Task UpdatePosition(Position position);
    public Task DeletePosition(Position position);
}