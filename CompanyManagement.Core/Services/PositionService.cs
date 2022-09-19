using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Core.Services;

public class PositionService : IPositionService
{
    private readonly DataContext _dataContext;

    public PositionService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<List<Position>> GetPositions(int pageNumber)
    {
        var itemsPerPage = 10;
        if (pageNumber < 0)
            pageNumber = 1;
        var skipItems = (pageNumber - 1) * itemsPerPage;
        return await _dataContext.Positions
            .OrderBy(p => p.Title)
            .Skip(skipItems)
            .Take(itemsPerPage)
            .ToListAsync();
    }

    public async Task<Position> GetPositionById(int id)
    {
        return await _dataContext.Positions.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Position> GetPositionByTitle(string title)
    {
        return await _dataContext.Positions.FirstOrDefaultAsync(p =>
            EF.Functions.ILike(p.Title, title));
    }

    public async Task<Position> CreatePosition(string title)
    {
        var position = new Position() { Title = title };
        var newPosition = _dataContext.Positions.Add(position);
        await _dataContext.SaveChangesAsync();
        return newPosition.Entity;
    }


    public async Task UpdatePosition(Position position)
    {
        _dataContext.Positions.Update(position);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeletePosition(Position position)
    {
        _dataContext.Positions.Remove(position);
        await _dataContext.SaveChangesAsync();
    }
}